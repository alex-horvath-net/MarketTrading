namespace MarketDataIngestionService.Features.IngestLiveMarketData;

public interface ITime { 
    DateTime UtcNow { get; }
}