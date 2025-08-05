using MarketDataIngestionService.Domain;

namespace MarketDataIngestionService.Features.IngestLiveMarketData; 
public interface IPublisher {
    Task Publish(IEnumerable<MarketPrice> liveDataBatch, string symbol, CancellationToken token);
}
