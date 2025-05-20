using Infrastructure.IdentityManager;
using Business.Experts.Trader;
using Infrastructure.Technology.Identity;
using TradingPortal.Blazor.Components;
using TradingPortal.Blazor.Components.Account.Pages;
using TradingPortal.Blazor.Components.Account.Pages.Manage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddHttpClient<ITraderApiClient, TraderApiClient>(client => {
    client.BaseAddress = new Uri("https://localhost:5001");  // API Gateway over HTTPS
});

builder.Services.AddOidcAuthentication(options => {
    options.ProviderOptions.Authority = "https://localhost:5002";  // IdentityService over HTTPS
    options.ProviderOptions.ClientId = "trading-ui";
    options.ProviderOptions.ResponseType = "code";
});

builder.Services
    .AddIdentityManager(builder.Configuration).AddIdentity(builder.Configuration)
    .AddTrader(builder.Configuration);
    

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseMigrationsEndPoint();
} else {
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints(
    ExternalLogin.LoginCallbackAction,
    ExternalLogins.LinkLoginCallbackAction);

app.Run();
