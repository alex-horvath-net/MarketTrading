using System.ComponentModel.DataAnnotations;
using Infrastructure.Adapters.Blazor;

namespace ApiGateway.Client.Trader.PlaceTrade;

public record PlaceTradeInputModel(string TraderId) : InputModel(TraderId) {

    [Required]
    public string Instrument { get; set; } = "";
    [Range(1, int.MaxValue)]
    public decimal Quantity { get; set; }
    public TradeSide Side { get; set; } = TradeSide.Buy;
    public OrderType OrderType { get; set; } = OrderType.Market;
    public bool OrderTypeIsNotMarket => OrderType != OrderType.Market;
    [Range(0.01, double.MaxValue)]
    public decimal? Price { get; set; }
    public TimeInForce TimeInForce { get; set; } = TimeInForce.Day;
    public string? StrategyCode { get; set; }
    public string? PortfolioCode { get; set; }
    public string? UserComment { get; set; }
    public DateTime? ExecutionRequestedFor { get; set; }
}
