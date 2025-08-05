using System.Collections.Concurrent;
using MarketDataIngestionService.Domain;
using MarketDataIngestionService.Features.IngestLiveMarketData;
using Microsoft.Extensions.Options;

namespace MarketDataIngestionService.Infrastructure.Buffer;
public class BlockingCollectionBuffer : IBuffer {
    private readonly BlockingCollection<MarketPrice> buffer;
    private readonly ILogger<BlockingCollectionBuffer> _logger;
    private long _bufferOverFlowCounter;
    private int _bufferSizeRecord = 0;

    private readonly BufferOptions _options;
    public BlockingCollectionBuffer(IOptions<BufferOptions> options, ILogger<BlockingCollectionBuffer> logger) {
        this.buffer = new BlockingCollection<MarketPrice>();
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }
    public void BufferLiveData(MarketPrice liveData, string instanceId) {
        try {

            if (buffer.TryAdd(liveData)) {
                var current = buffer.Count;
                if (current > _bufferSizeRecord) {
                    Interlocked.Exchange(ref _bufferSizeRecord, current);
                    _logger.LogWarning("Buffer size reached new record.");
                    MonitorBuffer(buffer, instanceId);
                }
            } else {
                Interlocked.Increment(ref _bufferOverFlowCounter);
                _logger.LogWarning("Buffer size overflow");
                MonitorBuffer(buffer, instanceId);
            }
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to buffer live data [InstanceId: {InstanceId}]", instanceId);
            MonitorBuffer(buffer, instanceId);
        }
    }


    public void CompleteAdding() {
         buffer.CompleteAdding();
    }



    private void MonitorBuffer(BlockingCollection<MarketPrice> buffer, string instanceId) {
        _logger.LogDebug("BufferSize: {BufferSize}; BufferCapacity: {BufferCapacity}; BufferSizeRecord: {BufferSizeRecord} BufferOverFlowCount: {BufferOverFlowCount}. [InstanceId: {InstanceId}]",
                    buffer.Count, buffer.BoundedCapacity, _bufferSizeRecord, _bufferOverFlowCounter, instanceId);
    }
}


public class BufferOptions {
    public int BufferCapacity { get; set; } = 100_000;
    public int BatchSizeTrashold { get; set; } = 50;
    public int PublishIntervalTrasholdMs { get; set; } = 100;
}
