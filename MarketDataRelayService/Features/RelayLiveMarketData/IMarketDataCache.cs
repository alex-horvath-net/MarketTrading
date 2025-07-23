using MarketDataRelayService.Domain;

namespace MarketDataRelayService.Features.RelayLiveMarketData;

public interface IMarketDataCache {
    void Set(MarketPrice price);
    MarketPrice? Get(string symbol);
    IReadOnlyCollection<MarketPrice> GetAll();
}
