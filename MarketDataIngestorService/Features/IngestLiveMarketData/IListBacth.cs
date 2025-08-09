using MarketDataIngestionService.Domain;

namespace MarketDataIngestionService.Features.IngestLiveMarketData; 
public interface IListBacth { 
    int Count { get; }
    bool Add(MarketPrice liveData, ref List<MarketPrice> batch);
}