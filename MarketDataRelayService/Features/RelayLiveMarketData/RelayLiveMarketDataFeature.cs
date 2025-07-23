namespace MarketDataRelayService.Features.RelayLiveMarketData;
public class RelayLiveMarketDataFeature : IRelayLiveMarketDataFeature {
    private readonly IMarketDataCache _cache;
    private readonly IMarketDataStorage _storage;
    private readonly IEventReader _eventReader;
    private readonly IClientNotifier _notifier;
    private readonly ISrorageEnqueue _storageEnqueue;
    private readonly ILogger<RelayLiveMarketDataFeature> _logger;

    public RelayLiveMarketDataFeature(
        IMarketDataCache cache,
        IMarketDataStorage storage,
        IEventReader consumer,
        IClientNotifier notifier,
        ISrorageEnqueue queue,
        ILogger<RelayLiveMarketDataFeature> logger) {
        _logger = logger;
        _eventReader = consumer;
        _notifier = notifier;
        _storageEnqueue = queue;
        _cache = cache;
        _storage = storage;
    }

    public async Task RunAsync(CancellationToken token) {

        await foreach (var marketData in _eventReader.ReadAsync(token)) {
            try {
                _cache.Set(marketData);

                _storageEnqueue.Enqueue(t => _storage.StoreAsync(marketData, t));

                await _notifier.SendAsync(marketData, token);
            } catch (Exception ex) {
                _logger.LogError(ex, "Failed to notify UI for {Symbol}", marketData.Symbol);
            }
        }
    }
}
