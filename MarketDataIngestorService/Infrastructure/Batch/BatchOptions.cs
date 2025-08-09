namespace MarketDataIngestionService.Infrastructure.Batch;

public class BatchOptions {
    public int Size { get; set; } = 100;
    public int IntervalMs { get; set; } = 1000;

}
