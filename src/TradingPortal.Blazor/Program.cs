using Business.Experts.IdentityManager;
using Business.Experts.Trader;
using Infrastructure.Technology.Identity;
using TradingPortal.Blazor.Components;
using TradingPortal.Blazor.Components.Account.Pages;
using TradingPortal.Blazor.Components.Account.Pages.Manage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddIdentityManager(builder.Configuration).AddIdentity(builder.Configuration);
builder.Services.AddTrader(builder.Configuration);

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
