namespace MarketDataIngestorService.Domain;

public class Symbol {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}