namespace ApiGateway.Client.Trader.PlaceTrade;

/// <summary>
/// Handles all client-side logic for placing a trade, including form binding and execution.
/// Convinient for UI
/// </summary>
public interface IPlaceTradeClient {
    PlaceTradeInputModel InputModel { get; set; }
    PlaceTradeViewModel ViewModel { get; set; }
    Task Execute(CancellationToken token);
}
