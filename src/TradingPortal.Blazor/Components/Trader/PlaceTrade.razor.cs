using ApiGateway.Client.Trader;
using IdentityService.Client;
using IdentityService.IdentityManager;
using Microsoft.AspNetCore.Components;

namespace TradingPortal.Blazor.Components.Trader;
public partial class PlaceTrade : ComponentBase {
    private string TraderId = string.Empty;

    private CancellationTokenSource tcs = new();
    private CancellationToken token => tcs.Token;

    [CascadingParameter]
    public HttpContext? httpContext { get; set; }

    [Inject]
    private IdentityManager identityManager { get; set; } = default!;

    [Inject]
    private ITraderServiceClient trader { get; set; } = default!;

    protected override async Task OnInitializedAsync() {
        TraderId = identityManager.GetUserName(httpContext);

        trader.PlaceTrade.InputModel = new(TraderId);
        trader.FindTrades.InputModel = new(TraderId);

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
