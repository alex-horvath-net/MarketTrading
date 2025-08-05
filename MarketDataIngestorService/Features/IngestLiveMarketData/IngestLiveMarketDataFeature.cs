using System.Collections.Concurrent;
using MarketDataIngestionService.Domain;
using MarketDataIngestionService.Infrastructure.Host;
using Microsoft.Extensions.Options;

namespace MarketDataIngestionService.Features.IngestLiveMarketData;


public class IngestLiveMarketDataFeature : IIngestLiveMarketDataFeature {
    private readonly IRepository _repository;
    private readonly IPublisher _publisher;
    private readonly IReceiver _receiver;
    private readonly ITime _time;
    private readonly ILogger<IngestLiveMarketDataFeature> _logger;
    private readonly IBuffer _buffer;
    private IEnumerable<string> _symbols = [];
    private long _missedTicks;
    private int _bufferSizeRecord = 0;
    private readonly string _instanceId = Guid.NewGuid().ToString();

    public IngestLiveMarketDataFeature(
        IReceiver receiver,
        IPublisher publisher,
        IRepository repository,
        IBuffer buffer,
        ILogger<IngestLiveMarketDataFeature> logger,
        ITime time) {
        _receiver = receiver;
        _publisher = publisher;
        _repository = repository;
        _logger = logger;
        _buffer = buffer;
        _time = time;
    }

    public async Task RunAsync(CancellationToken token) {
        _logger.LogInformation("MarketDataIngestionService starting — InstanceId: {InstanceId}", _instanceId);

        _symbols = await _repository.LoadSymbols(token);

        if (_symbols.Count() == 0) {
            _logger.LogError("Loaded symbol list is empty or contains only invalid entries — terminating.");

            return;
        }

        var publishTask = Task.Run(() => PublishLiveData(token), token);
        //var monitorTask = Task.Run(() => MonitorBuffer(token), token);

        await _receiver.StartReceivingLiveData(_symbols,  _instanceId,  token);


        //await Task.WhenAll(publishTask, monitorTask);

        _logger.LogInformation("Dropped tick count during run: {Count} [InstanceId: {InstanceId}]", _missedTicks, _instanceId);
        _logger.LogInformation("Max buffer usage reached: {Max} / {Capacity} [InstanceId: {InstanceId}]", _bufferSizeRecord, _buffer.BoundedCapacity, _instanceId);
        _logger.LogInformation("MarketDataIngestionService stopped — InstanceId: {InstanceId}", _instanceId);
    }


    private async Task PublishLiveData(CancellationToken token) {
        var liveDataBatch = new List<MarketPrice>(_options.BatchSizeTrashold);
        var publishIntervalTrashold = TimeSpan.FromMilliseconds(_options.PublishIntervalTrasholdMs);
        var lastPublish = _time.UtcNow;

        foreach (var liveData in _buffer.GetConsumingEnumerable(token)) {
            liveDataBatch.Add(liveData);

            bool sizeTrigger = liveDataBatch.Count >= _options.BatchSizeTrashold;
            bool timeTrigger = _time.UtcNow - lastPublish >= publishIntervalTrashold;
            if (sizeTrigger || timeTrigger) {
                await PublishBatch(liveDataBatch, token);
                liveDataBatch.Clear();
                lastPublish = _time.UtcNow;
            }
        }

        // Flush remaining messages at the end
        if (liveDataBatch.Count > 0) {
            await PublishBatch(liveDataBatch, token);
        }
    }

    private async Task PublishBatch(List<MarketPrice> batch, CancellationToken token) {
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
}

