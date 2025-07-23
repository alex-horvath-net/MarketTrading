namespace MarketDataIngestionService.Infrastructure.Database.Models;

public class BloombergSymbol {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}