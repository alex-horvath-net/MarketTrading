using System.Collections.Concurrent;
using MarketDataRelayService.Domain;
using MarketDataRelayService.Features.RelayLiveMarketData;

namespace MarketDataRelayService.Infrastructure;

public class InMemoryMarketDataCache : IMarketDataCache {
    private readonly ConcurrentDictionary<string, MarketPrice> _cache = new();

    public void Set(MarketPrice price) =>
        _cache[price.Symbol] = price;

    public MarketPrice? Get(string symbol) =>
        _cache.TryGetValue(symbol, out var price) ? price : null;

    public IReadOnlyCollection<MarketPrice> GetAll() => _cache.Values.ToList();
}