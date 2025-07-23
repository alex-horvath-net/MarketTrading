namespace MarketDataIngestionService.Features.LiveMarketData;

public class IngestLiveMarketDataOptions {
    public int BufferCapacity { get; set; } = 100_000;
    public int BatchSize { get; set; } = 50;
    public int FlushIntervalMs { get; set; } = 100;
}
 