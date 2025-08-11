using System.Collections.Concurrent;
using MarketDataIngestionService.Domain;
using MarketDataIngestionService.Features.IngestLiveMarketData;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;

namespace MarketDataIngestionService.Infrastructure.Buffer;

public class BlockingCollectionBuffer : IBuffer {
    private readonly BlockingCollection<MarketPrice> _buffer;
    private readonly ILogger<BlockingCollectionBuffer> _logger;
    private long _overFlowCounter;
    private int _maxSize = 0;

    public BufferOptions Options { get; private set; }
    public BlockingCollectionBuffer(IOptions<BufferOptions> options, ILogger<BlockingCollectionBuffer> logger) {
        Options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _buffer = new BlockingCollection<MarketPrice>(Options.BufferCapacity);
    }

    public async IAsyncEnumerable<MarketPrice> GetItemsAsync([EnumeratorCancellation] CancellationToken cancellationToken) {
        while (!cancellationToken.IsCancellationRequested) {
            MarketPrice item;
            try {
                item = _buffer.Take(cancellationToken);
            } catch (OperationCanceledException) {
                yield break;
            } catch (InvalidOperationException) {
                // Buffer is marked as completed and empty
                yield break;
            }
            yield return item;  
        }
        await Task.CompletedTask;
    }

    public void AddItem(MarketPrice liveData, string hostId) {
        try {
            if (_buffer.TryAdd(liveData)) {
                var currentSize = _buffer.Count;
                if (currentSize > _maxSize) {
                    Interlocked.Exchange(ref _maxSize, currentSize);
                    _logger.LogWarning("Buffer max size is {MaxSize}. [HostId: {HostId}]", _maxSize, hostId);
                    MonitorBuffer(_buffer, hostId);
                }
            } else {
                Interlocked.Increment(ref _overFlowCounter);
                _logger.LogWarning("Buffer overflow happened at the {OverFlowCounter} times. [HostId: {HostId}]", _overFlowCounter, hostId);
                MonitorBuffer(_buffer, hostId);
            }
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to buffer live data. [HostId: {HostId}]", hostId);
            MonitorBuffer(_buffer, hostId);
            throw;
        }
    }

    public void StopAddItem() {
        _buffer.CompleteAdding();
    }

    private void MonitorBuffer(BlockingCollection<MarketPrice> buffer, string hostId) {
        _logger.LogDebug("BufferCurrentSize: {BufferCurrentSize}; BufferCapacity: {BufferCapacity}; BufferMaxSize: {BufferMaxSize} er: {BufferOverFlowCounter}. [HostId: {HostId}]",
            buffer.Count, buffer.BoundedCapacity, _maxSize, _overFlowCounter, hostId);
    }
}
