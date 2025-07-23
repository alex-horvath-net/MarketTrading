namespace MarketDataRelayService.Features.RelayLiveMarketData;
public class RelayLiveMarketDataFeature : IRelayLiveMarketDataFeature {
    private readonly IMarketDataCache _cache;
    private readonly IMarketDataStorage _storage;
    private readonly IEventHubConsumer _consumer;
    private readonly IClientNotifier _notifier;
    private readonly IQueue _queue;
    private readonly ILogger<RelayLiveMarketDataFeature> _logger;

    public RelayLiveMarketDataFeature(
        IMarketDataCache cache,
        IMarketDataStorage storage,
        IEventHubConsumer consumer,
        IClientNotifier notifier,
        IQueue queue,
        ILogger<RelayLiveMarketDataFeature> logger) {
        _logger = logger;
        _consumer = consumer;
        _notifier = notifier;
        _queue = queue;
        _cache = cache;
        _storage = storage;
    }

    public async Task RunAsync(CancellationToken token) {

        await foreach (var liveData in _consumer.ReadAsync(token)) {
            try {
                _cache.Set(liveData);

                _queue.Enqueue(t => _storage.StoreAsync(liveData, t));

                await _notifier.SendAsync(liveData, token);
            } catch (Exception ex) {
                _logger.LogError(ex, "Failed to notify UI for {Symbol}", liveData.Symbol);
            }
        }
    }
}
