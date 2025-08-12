using MarketDataIngestionService.Domain;

namespace MarketDataIngestionService.Features.IngestLiveMarketData; 
public interface IListBacth { 
    int Count { get; }
    bool IsReadyToPublished(MarketPrice liveData, ref List<MarketPrice> batch);
}