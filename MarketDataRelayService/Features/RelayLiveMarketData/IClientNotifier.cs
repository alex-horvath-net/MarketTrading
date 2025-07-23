using MarketDataRelayService.Domain;

namespace MarketDataRelayService.Features.RelayLiveMarketData;

public interface IClientNotifier {
    Task SendAsync(MarketPrice price, CancellationToken token);
}

