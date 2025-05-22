using IdentityService.Client;
using Infrastructure.Identity;
using Infrastructure.Technology.Identity;
using TradingPortal.Blazor.Components;

var builder = WebApplication.CreateBuilder(args);

// Razor + SignalR
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

// Token Forwarding + OIDC + cookie authentication 
builder.Services.AddIdentityInfrastructure(builder.Configuration);

// Typed HttpClient for IdentityService microservice
builder.Services.AddIdentityClient(builder.Configuration);


// Typed HttpClient for API Gateway (e.g. Trader microservice)
builder.Services.AddApiGatewayClient(builder.Configuration);

// Register domain experts
builder.Services.AddTrader(builder.Configuration);

var app = builder.Build();

// Environment-specific middleware
if (app.Environment.IsDevelopment()) {
    app.UseMigrationsEndPoint();
} else {
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

// Optional: exposes login/logout/profile/etc. from Identity UI
app.MapAdditionalIdentityEndpoints();

app.Run();



//// Typed HTTP client for API Gateway (YARP)
//builder.Services.AddHttpClient<ITraderApiClient, TraderApiClient>(client => {
//    client.BaseAddress = new Uri("https://localhost:5001");  // API Gateway over HTTPS
//});

//// OpenID Connect → IdentityService
//builder.Services.AddOidcAuthentication(options => {
//    options.ProviderOptions.Authority = "https://localhost:5002";  // Identity microservice
//    options.ProviderOptions.ClientId = "trading-ui";
//    options.ProviderOptions.ResponseType = "code";
//});

//// Domain experts & dependencies
//builder.Services
//    .AddIdentityManager(builder.Configuration)
//    .AddIdentity(builder.Configuration)
//    .AddTrader(builder.Configuration);

//var app = builder.Build();

//// Env-specific setup
//if (app.Environment.IsDevelopment()) {
//    app.UseMigrationsEndPoint();
//} else {
//    app.UseExceptionHandler("/Error", createScopeForErrors: true);
//    app.UseHsts(); // Add secure headers
//}

//app.UseHttpsRedirection();         // 🔐 Enforce HTTPS
//app.UseStaticFiles();              // Required for serving CSS/JS/assets
//app.UseAntiforgery();              // CSRF protection

//app.MapStaticAssets();
//app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

//// Identity UI Endpoints
//app.MapAdditionalIdentityEndpoints(
//    ExternalLogin.LoginCallbackAction,
//    ExternalLogins.LinkLoginCallbackAction);

//app.Run();
