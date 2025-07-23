using System.Collections.Concurrent;
using MarketDataIngestionService.Domain;
using MarketDataIngestionService.Features.AcquiereLiveMarketData;
using Microsoft.Extensions.Options;

namespace MarketDataIngestionService.Features.LiveMarketData;


public class IngestLiveMarketDataFeature : IIngestLiveMarketDataFeature {
    private readonly IRepository _repository;
    private readonly IPublisher _publisher;
    private readonly IReceiver _receiver;
    private readonly ILogger<IngestLiveMarketDataFeature> _logger;
    private readonly IngestLiveMarketDataOptions _options;
    private readonly BlockingCollection<MarketPrice> _buffer;
    private IEnumerable<string> _symbols = [];
    private long _droppedTicks;
    private int _bufferHighWaterMark = 0;
    private readonly string _instanceId = Guid.NewGuid().ToString();

    public IngestLiveMarketDataFeature(
        IReceiver receiver,
        IPublisher publisher,
        IRepository repository,
        ILogger<IngestLiveMarketDataFeature> logger,
        IOptions<IngestLiveMarketDataOptions> options) {
        _receiver = receiver;
        _publisher = publisher;
        _repository = repository;
        _logger = logger;
        _options = options.Value;
        _buffer = new BlockingCollection<MarketPrice>(_options.BufferCapacity);
    }

    public async Task RunAsync(CancellationToken token) {
        _logger.LogInformation("MarketDataIngestionService starting — InstanceId: {InstanceId}", _instanceId);

        _symbols = await _repository.LoadSymbols(token);

        if (_symbols.Count() == 0) {
            _logger.LogError("Loaded symbol list is empty or contains only invalid entries — terminating.");

            return;
        }

        var publishTask = Task.Run(() => PublishLiveData(token), token);
        var monitorTask = Task.Run(() => MonitorBuffer(token), token);

        await _receiver.Receive(_symbols, LiveDataReceived, token);

        _buffer.CompleteAdding();

        await Task.WhenAll(publishTask, monitorTask);

        _logger.LogInformation("Dropped tick count during run: {Count} [InstanceId: {InstanceId}]", _droppedTicks, _instanceId);
        _logger.LogInformation("Max buffer usage reached: {Max} / {Capacity} [InstanceId: {InstanceId}]", _bufferHighWaterMark, _buffer.BoundedCapacity, _instanceId);
        _logger.LogInformation("MarketDataIngestionService stopped — InstanceId: {InstanceId}", _instanceId);
    }

    private void LiveDataReceived(MarketPrice liveData) {
        try {
            if (!IsValid(liveData)) {
                _logger.LogWarning("Invalid live data received: {@Price} [InstanceId: {InstanceId}]", liveData, _instanceId);
                return;
            }

            if (_buffer.TryAdd(liveData)) {
                var current = _buffer.Count;
                Interlocked.Exchange(ref _bufferHighWaterMark, Math.Max(_bufferHighWaterMark, current));
            } else {
                Interlocked.Increment(ref _droppedTicks);
                _logger.LogWarning("Buffer overflow — dropped live data for {Symbol} [InstanceId: {InstanceId}]",
                    liveData.Symbol, _instanceId);
            }
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to receive live data [InstanceId: {InstanceId}]", _instanceId);
        }
    }

    private async Task PublishLiveData(CancellationToken token) {
        var liveDataBatch = new List<MarketPrice>(_options.BatchSize);
        var flushInterval = TimeSpan.FromMilliseconds(_options.FlushIntervalMs);
        var lastFlush = DateTime.UtcNow;

        foreach (var liveData in _buffer.GetConsumingEnumerable(token)) {
            liveDataBatch.Add(liveData);

            bool sizeLimitReached = liveDataBatch.Count >= _options.BatchSize;
            bool timeLimitReached = (DateTime.UtcNow - lastFlush) >= flushInterval;

            if (sizeLimitReached || timeLimitReached) {
                await FlushBatchByPartitionKey(liveDataBatch, token);
                liveDataBatch.Clear();
                lastFlush = DateTime.UtcNow;
            }
        }

        // Flush remaining messages at the end
        if (liveDataBatch.Count > 0) {
            await FlushBatchByPartitionKey(liveDataBatch, token);
        }
    }

    private async Task FlushBatchByPartitionKey(List<MarketPrice> batch, CancellationToken token) {
        var grouped = batch.GroupBy(p => p.Symbol.Replace(" ", "").ToUpperInvariant());

        foreach (var group in grouped) {
            var partitionKey = group.Key;
            var events = group.ToList();

            try {
                await _publisher.Publish(events, partitionKey, token);
                _logger.LogDebug("Published {Count} events to partition {Symbol}", events.Count, partitionKey);
            } catch (Exception ex) {
                _logger.LogError(ex, "Failed to publish {Count} events to partition {Symbol}", events.Count, partitionKey);
            }
        }
    }

    private async Task MonitorBuffer(CancellationToken token) {
        while (!token.IsCancellationRequested) {
            _logger.LogInformation("Buffer: {Count}/{Capacity}, Dropped: {Dropped}, MaxUsed: {HighWaterMark} [InstanceId: {InstanceId}]",
               _buffer.Count,
               _buffer.BoundedCapacity,
               _droppedTicks,
               _bufferHighWaterMark,
               _instanceId);

            await Task.Delay(TimeSpan.FromSeconds(60), token);
        }
    }

    private static bool IsValid(MarketPrice price) => !string.IsNullOrWhiteSpace(price.Symbol)
        && price.Timestamp != default
        && price.Bid >= 0
        && price.Ask >= 0
        && price.Last >= 0
        && price.Bid <= 1_000_000
        && price.Ask <= 1_000_000
        && price.Last <= 1_000_000;
}


public class IngestLiveMarketDataOptions {
    public int BufferCapacity { get; set; } = 100_000;
    public int BatchSize { get; set; } = 50;
    public int FlushIntervalMs { get; set; } = 100;
}
