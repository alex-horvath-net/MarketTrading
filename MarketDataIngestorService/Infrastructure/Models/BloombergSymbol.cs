namespace MarketDataIngestorService.Infrastructure.Models;

public class BloombergSymbol {
    public int Id { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}