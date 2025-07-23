using MarketDataIngestionService.Domain;
using MarketDataIngestionService.Features.LiveMarketData;

namespace MarketDataIngestionService.Infrastructure.LiveDataReceiver;

public class FakeLReceiver : IReceiver {
    public async Task Receive(IEnumerable<string> symbols, Action<MarketPrice> onNewLiveDataReceived, CancellationToken token) { 
        var rand = new Random();
        while (!token.IsCancellationRequested) {
            foreach (var symbol in symbols) {
                // raw live data 
                var bid = rand.NextDouble() * 100;
                var ask = rand.NextDouble() * 100;
                var last = rand.NextDouble() * 100;
                var timestamp = DateTime.UtcNow;
                var correlationId = Guid.NewGuid().ToString();

                // domain tick daat
                var liveData = new MarketPrice {
                    Symbol = symbol, 
                    Bid = bid,
                    Ask = ask, 
                    Last = last,
                    Timestamp = timestamp,
                    CorrelationId = correlationId   
                };
                 
                onNewLiveDataReceived(liveData);
            }

            await Task.Delay(1000, token);
        }
    }
}

 