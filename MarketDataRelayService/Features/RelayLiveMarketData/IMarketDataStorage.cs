using MarketDataRelayService.Domain;

namespace MarketDataRelayService.Features.RelayLiveMarketData;

public interface IMarketDataStorage {
    Task StoreAsync(MarketPrice price, CancellationToken cancellationToken);
}
