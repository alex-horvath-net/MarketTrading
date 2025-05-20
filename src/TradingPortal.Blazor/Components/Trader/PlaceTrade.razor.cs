using Infrastructure.IdentityManager;
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
    private CancellationToken token => tcs.Token;

    protected override async Task OnInitializedAsync() {
        userName = identityManager.GetUserName(HttpContext);

        trader.PlaceTrade.InputModel = new(userName);
        trader.FindTrades.InputModel = new(userName);

        await trader.FindTrades.Execute(token);
    }

    private async Task PlaceTradeClick() {
        await trader.PlaceTrade.Execute(token);
        await trader.FindTrades.Execute(token);
    }

    private async Task TradesFilterClick() {
        await trader.FindTrades.Execute(token);
    }
}
