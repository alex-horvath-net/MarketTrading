using Infrastructure.Adapters.Blazor;
using TradingService.Domain;

namespace ApiGateway.Client.Trader.FindTrades;
public record FindTradesViewModel : ViewModel {
    public TableModel<Trade> Trades { get; set; } = new();
    public int TotalCount { get; set; }
    public int BuyCount { get; set; }
    public int SellCount { get; set; }
}