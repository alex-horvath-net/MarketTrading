using MarketDataIngestionService.Domain;

namespace MarketDataIngestionService.Features.IngestLiveMarketData; 
public interface IPublisher {
    Task StartPublishingLiveData(string hostId, CancellationToken token);
}
