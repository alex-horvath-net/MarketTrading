using System.ComponentModel;
using Infrastructure.Adapters.Blazor;

namespace ApiGateway.Client.Trader.PlaceTrade;

public record PlaceTradeViewModel : ViewModel {
    public TradeVM Trade { get; set; }
}

public record TradeVM {
    [DisplayName("ID")]
    public string Id { get; set; }
    public string TraderId { get; set; }
    public string Instrument { get; set; }
    public string Side { get; set; }
    public decimal Quantity { get; set; }
    public decimal? Price { get; set; }
    public string OrderType { get; set; }
    public string TimeInForce { get; set; }
    public DateTime SubmittedAt { get; set; }
    public string? StrategyCode { get; set; }
    public string? PortfolioCode { get; set; }
    public string? UserComment { get; set; }
    public DateTime? ExecutionRequestedForUtc { get; set; }
    public string Status { get; set; }
}
