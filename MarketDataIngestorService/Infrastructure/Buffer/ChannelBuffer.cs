using System.Runtime.CompilerServices;
using System.Threading.Channels;
using MarketDataIngestionService.Domain;
using MarketDataIngestionService.Features.IngestLiveMarketData;
using Microsoft.Extensions.Options;

namespace MarketDataIngestionService.Infrastructure.Buffer;

public class ChannelBuffer : IBuffer {
    private readonly Channel<MarketPrice> _channel;
    private readonly ILogger<ChannelBuffer> _logger;
    private readonly BufferOptions _options;

    private long _overflowCounter;
    private int _maxSize;
    private long _totalAdded;
    private long _totalRead;

    public BufferOptions Options => _options;

    public ChannelBuffer(IOptions<BufferOptions> options, ILogger<ChannelBuffer> logger) {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        var boundedOptions = new BoundedChannelOptions(_options.BufferCapacity) {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = true,
            SingleWriter = false
        };

        _channel = Channel.CreateBounded<MarketPrice>(boundedOptions);
    }

    public async IAsyncEnumerable<MarketPrice> GetItemsAsync([EnumeratorCancellation] CancellationToken cancellationToken) {
        while (await _channel.Reader.WaitToReadAsync(cancellationToken)) {
            while (_channel.Reader.TryRead(out var item)) {
                Interlocked.Increment(ref _totalRead);
                yield return item;
            }
        }
        _logger.LogInformation("Buffer read completed. TotalRead: {TotalRead}", _totalRead);
    }

    public void AddItem(MarketPrice liveData, string instanceId) {
        if (_channel.Writer.TryWrite(liveData)) {
            Interlocked.Increment(ref _totalAdded);
            var currentSize = _channel.Reader.Count;
            if (currentSize > _maxSize) {
                Interlocked.Exchange(ref _maxSize, currentSize);
                _logger.LogWarning("Buffer max size is {MaxSize}. [InstanceId: {InstanceId}]", _maxSize, instanceId);
                MonitorBuffer(instanceId);
            }
        } else {
            Interlocked.Increment(ref _overflowCounter);
            _logger.LogWarning("Buffer overflow happened {OverflowCounter} times. [InstanceId: {InstanceId}]", _overflowCounter, instanceId);
            MonitorBuffer(instanceId);
        }
    }

    public void StopAddItem() {
        _channel.Writer.Complete();
        _logger.LogInformation("Buffer stopped. TotalAdded: {TotalAdded}, TotalRead: {TotalRead}, Overflow: {OverflowCounter}", _totalAdded, _totalRead, _overflowCounter);
    }

    private void MonitorBuffer(string instanceId) {
        _logger.LogDebug("BufferCurrentSize: {CurrentSize}; BufferCapacity: {Capacity}; BufferMaxSize: {MaxSize}; OverflowCount: {Overflow}. [InstanceId: {InstanceId}]",
            _channel.Reader.Count, _options.BufferCapacity, _maxSize, _overflowCounter, instanceId);
    }

    public IEnumerable<MarketPrice> GetItems(CancellationToken token) {
        throw new NotImplementedException();
    }
}
