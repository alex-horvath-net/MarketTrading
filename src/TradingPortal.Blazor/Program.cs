using ApiGateway.Client.Trader;
using IdentityService.Client;
using Infrastructure.Identity;
using Infrastructure.IdentityManager;
using Infrastructure.Technology.Identity;
using TradingPortal.Blazor.Components;
using TradingPortal.Blazor.Components.Account.Pages;
using TradingPortal.Blazor.Components.Account.Pages.Manage;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Services.AddRazorComponents().AddInteractiveServerComponents(); // Razor + SignalR

builder.Services.AddIdentityInfrastructure(config); // Token Forwarding + OIDC + cookie authentication 
builder.Services.AddIdentityManager(config);
builder.Services.AddIdentityClient(config); // Typed HttpClient for IdentityService microservice

builder.Services.AddApiGatewayClient(config); // Typed HttpClient for API Gateway 


//builder.Services.AddTrader(confign); // Register domain experts

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseMigrationsEndPoint();
} else {
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts(); // Add secure headers
}

app.UseHttpsRedirection(); // 🔐 Enforce HTTPS
app.UseStaticFiles();   // Required for serving CSS/JS/assets
app.UseAntiforgery();  // CSRF protection

app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

// Optional: exposes login/logout/profile/etc. from Identity UI
// Identity UI Endpoints
app.MapAdditionalIdentityEndpoints(
    ExternalLogin.LoginCallbackAction,
    ExternalLogins.LinkLoginCallbackAction);

app.Run();