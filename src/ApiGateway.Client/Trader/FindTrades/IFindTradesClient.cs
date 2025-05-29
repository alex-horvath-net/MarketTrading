namespace ApiGateway.Client.Trader.FindTrades;
/// <summary>
/// Handles filtering and loading of recent or historical trades.
/// </summary>
public interface IFindTradesClient {
    FindTradesInputModel InputModel { get; set; }
    FindTradesViewModel ViewModel { get; }

    Task Execute(CancellationToken token = default);
} 