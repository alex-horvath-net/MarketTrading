using MarketDataIngestionService.Features.IngestLiveMarketData;

namespace MarketDataIngestionService.Infrastructure.Time;
public class LondonTime : ITime {
    public DateTime UtcNow => DateTime.Now;
}
