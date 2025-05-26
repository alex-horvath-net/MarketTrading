using ApiGateway.Client.Trader.FindTrades;
using ApiGateway.Client.Trader.PlaceTrade;

namespace ApiGateway.Client.Trader;

/// <summary>
/// Root interface for all trading-related client operations.
/// </summary>
public interface ITraderServiceClient {
    IPlaceTradeClient PlaceTrade { get; }
    IFindTradesClient FindTrades { get; }
}
