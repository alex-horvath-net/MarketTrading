using MarketDataIngestionService.Domain;

namespace MarketDataIngestionService.Features.IngestLiveMarketData; 
public interface IPublisher {
    Task PublishLiveData(string hostId, CancellationToken token);
}
