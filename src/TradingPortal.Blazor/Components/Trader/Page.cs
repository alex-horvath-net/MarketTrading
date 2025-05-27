using Infrastructure.IdentityManager;
using Microsoft.AspNetCore.Components;

namespace TradingPortal.Blazor.Components.Trader;

public class Page : ComponentBase {
    public string userName = string.Empty;

    public CancellationTokenSource tcs = new();
    public CancellationToken token => tcs.Token;

    [CascadingParameter]
    public HttpContext? HttpContext { get; set; }

    [Inject]
    public IdentityManager identityManager { get; set; } = default!;
}