using Business.Experts.IdentityManager;
using Business.Experts.Trader;
using Microsoft.AspNetCore.Components;

namespace TradingPortal.Blazor.Components.Trader;
public partial class PlaceTrade : ComponentBase {
    private string userName = string.Empty;

    [CascadingParameter]
    public HttpContext? HttpContext { get; set; }

    [Inject]
    private IdentityManager identityManager { get; set; } = default!;

    [Inject]
    private Business.Experts.Trader.Trader trader { get; set; } = default!;

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
