namespace ApiGateway.Client;

public class TradeDto {
    public Guid Id { get; set; }
    public string Instrument { get; set; } = string.Empty;
    public string Side { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal? Price { get; set; }
    public string OrderType { get; set; } = string.Empty;
    public string TimeInForce { get; set; } = string.Empty;
    public DateTime SubmittedAtUtc { get; set; }
    public string? StrategyCode { get; set; }
    public string? PortfolioCode { get; set; }
    public string? UserComment { get; set; }
}
