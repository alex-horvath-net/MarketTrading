using MarketDataIngestionService.Domain;

namespace MarketDataIngestionService.Features.IngestLiveMarketData; 
public interface IPublisher {
    Task PublishLiveData(CancellationToken token);
}
