using MarketDataRelayService.Domain;

namespace MarketDataRelayService.Features.RelayLiveMarketData;

public interface IEventHubConsumer {
    IAsyncEnumerable<MarketPrice> ReadAsync(CancellationToken token);
}

