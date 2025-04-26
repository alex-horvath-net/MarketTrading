using Microsoft.AspNetCore.Components;

namespace TradingPortal.Blazor.Components.Trader;
public partial class PlaceTrade : ComponentBase {
    private string userName = string.Empty;

    [CascadingParameter]
    public HttpContext? HttpContext { get; set; }
    private CancellationTokenSource tcs = new();
    private CancellationToken Token => tcs.Token;

    protected override async Task OnInitializedAsync() {
        userName = identityManager.GetUserName(HttpContext);

        trader.PlaceTrade.InputModel = new(userName);
        trader.FindTrades.InputModel = new(userName);

        await trader.FindTrades.Execute(Token);
    }

    private async Task PlaceTradeClick() {
        await trader.PlaceTrade.Execute(Token);
        await trader.FindTrades.Execute(Token);
    }

    private async Task TradesFilterClick() {
        await trader.FindTrades.Execute(Token);
    }
}