using MarketDataRelayService.Domain;

namespace MarketDataRelayService.Features.RelayLiveMarketData;

public interface IEventReader {
    IAsyncEnumerable<MarketPrice> ReadAsync(CancellationToken token);
}

