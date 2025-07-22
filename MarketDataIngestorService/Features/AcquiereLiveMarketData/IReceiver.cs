using MarketDataIngestorService.Domain;

namespace MarketDataIngestorService.Features.LiveMarketData;

public interface IReceiver {
    Task Receive(IEnumerable<string> symbols, Action<MarketPrice> onNewLiveDataReceived, CancellationToken token);
}

 