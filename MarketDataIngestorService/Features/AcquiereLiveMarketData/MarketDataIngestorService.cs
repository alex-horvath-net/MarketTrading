using System.Collections.Concurrent;
using MarketDataIngestorService.Domain;
using MarketDataIngestorService.Features.AcquiereLiveMarketData;
using MarketDataIngestorService.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace MarketDataIngestorService.Features.LiveMarketData;


public interface IMarketDataPublisher {
    Task RunAsync(CancellationToken cancellationToken);
}

public class MarketDataIngestorService : IMarketDataPublisher {
    private readonly IRepository _repository;
    private readonly IPublisher _publisher;
    private readonly IReceiver _receiver;
    private readonly ILogger<MarketDataIngestorService> _logger;
    private readonly BlockingCollection<MarketPrice> _buffer = new(100_000);
    private IEnumerable<string> _symbols = [];
    private long _droppedTicks;
    private int _bufferHighWaterMark = 0;
    private readonly string _instanceId = Guid.NewGuid().ToString();

    public MarketDataIngestorService(
        IReceiver receiver,
        IPublisher publisher,
        IRepository repository,
        ILogger<MarketDataIngestorService> logger) {
        _receiver = receiver;
        _publisher = publisher;
        _repository = repository;
        _logger = logger;

    }

    public async Task RunAsync(CancellationToken token) {
        _logger.LogInformation("MarketDataIngestorService starting — InstanceId: {InstanceId}", _instanceId);

        _symbols = await _repository.LoadSymbols(token);

        if (_symbols.Count() == 0) {
            _logger.LogError("Loaded symbol list is empty or contains only invalid entries — terminating.");

            return;
        }

        var publishTask = Task.Run(() => LiveDataPublished(token), token);
        var monitorTask = Task.Run(() => MonitorBuffer(token), token);

        await _receiver.Receive(_symbols, LiveDataReceived, token);

        _buffer.CompleteAdding();

        await Task.WhenAll(publishTask, monitorTask);

        _logger.LogInformation("Dropped tick count during run: {Count} [InstanceId: {InstanceId}]", _droppedTicks, _instanceId);
        _logger.LogInformation("Max buffer usage reached: {Max} / {Capacity} [InstanceId: {InstanceId}]", _bufferHighWaterMark, _buffer.BoundedCapacity, _instanceId);
        _logger.LogInformation("MarketDataIngestorService stopped — InstanceId: {InstanceId}", _instanceId);
    }

    private void LiveDataReceived(MarketPrice liveData) {
        try {
            if (!IsValid(liveData)) {
                _logger.LogWarning("Rejected invalid tick: {@Price} [InstanceId: {InstanceId}]", liveData, _instanceId);
                return;
            }

            if (_buffer.TryAdd(liveData)) {
                var current = _buffer.Count;
                Interlocked.Exchange(ref _bufferHighWaterMark, Math.Max(_bufferHighWaterMark, current));
            } else {
                Interlocked.Increment(ref _droppedTicks);
                _logger.LogWarning("Buffer overflow — dropped event for {Symbol} [InstanceId: {InstanceId}]",
                    liveData.Symbol, _instanceId);
            }


            if (!_buffer.TryAdd(liveData)) {
                Interlocked.Increment(ref _droppedTicks);
                _logger.LogWarning("Buffer overflow — dropped event for {Symbol} [InstanceId: {InstanceId}]", liveData.Symbol, _instanceId);
            }

        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to process liveData event [InstanceId: {InstanceId}]", _instanceId);
        }
    }

    private async Task LiveDataPublished(CancellationToken token) {
        foreach (var liveData in _buffer.GetConsumingEnumerable(token)) {
            try {
                await _publisher.SendAsync(liveData, token);
                _logger.LogDebug("Event sent for {Symbol} [InstanceId: {InstanceId}]", liveData.Symbol, _instanceId);
            } catch (Exception ex) {
                _logger.LogError(ex, "Final failure to send event for {Symbol} [InstanceId: {InstanceId}]", liveData.Symbol, _instanceId);
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
