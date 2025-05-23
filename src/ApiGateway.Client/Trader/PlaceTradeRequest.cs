namespace ApiGateway.Client.Trader;

public class PlaceTradeRequest {
    public string Instrument { get; set; } = string.Empty;
    public string Side { get; set; }
    public decimal Quantity { get; set; }
    public decimal? Price { get; set; }
    public string OrderType { get; set; }
    public string TimeInForce { get; set; }
    public string? StrategyCode { get; set; }
    public string? PortfolioCode { get; set; }
    public string? UserComment { get; set; }
    public DateTime? ExecutionRequestedForUtc { get; set; }
}
