namespace MarketDataIngestionService.Infrastructure.Buffer;

public class BufferOptions {
    public int BufferCapacity { get; set; } = 100_000;
    public int BatchSizeThreshold { get; set; } = 50;
    public int PublishIntervalTrasholdMs { get; set; } = 100;
}
