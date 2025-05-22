namespace ApiGateway.Client.Trader;

public class PlaceTradeRequest {
    public string Instrument { get; set; } = string.Empty;
    public TradeSide Side { get; set; }
    public decimal Quantity { get; set; }
    public decimal? Price { get; set; }
    public OrderType OrderType { get; set; }
    public TimeInForce TimeInForce { get; set; }
    public string? StrategyCode { get; set; }
    public string? PortfolioCode { get; set; }
    public string? UserComment { get; set; }
    public DateTime? ExecutionRequestedForUtc { get; set; }
}
