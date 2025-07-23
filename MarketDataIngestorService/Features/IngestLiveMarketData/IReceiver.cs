using MarketDataIngestionService.Domain;

namespace MarketDataIngestionService.Features.LiveMarketData;

public interface IReceiver {
    Task Receive(IEnumerable<string> symbols, Action<MarketPrice> onNewLiveDataReceived, CancellationToken token);
}

 