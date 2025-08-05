using System.Collections.Concurrent;
using MarketDataIngestionService.Domain;

namespace MarketDataIngestionService.Features.IngestLiveMarketData;

public interface IReceiver {
    Task StartReceivingLiveData(IEnumerable<string> symbols, string instanceId, CancellationToken token);
}

 