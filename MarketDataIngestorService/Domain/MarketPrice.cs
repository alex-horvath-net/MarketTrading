namespace MarketDataIngestorService.Domain;
public class MarketPrice {
    public string Symbol { get; set; } = string.Empty;
    public double Bid { get; set; }
    public double Ask { get; set; }
    public double Last { get; set; }
    public DateTime Timestamp { get; set; }
    public string CorrelationId { get; set; } = string.Empty;
}
