using ApiGateway.Client.Trader.FindTrades;
using ApiGateway.Client.Trader.PlaceTrade;

namespace ApiGateway.Client.Trader;

public class TraderServiceClient : ITraderServiceClient {
    public IPlaceTradeClient PlaceTrade { get; }
    public IFindTradesClient FindTrades { get; }

    public TraderServiceClient(HttpClient http) {
        PlaceTrade = new PlaceTradeClient(http);
        FindTrades = new FindTradesClient(http);
    }
}