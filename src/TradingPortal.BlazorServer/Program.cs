using Microsoft.AspNetCore.DataProtection;
using TradingPortal.BlazorServer.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

var keyRingPath = Path.Combine(builder.Environment.ContentRootPath, "..", "keys"); // put keys folder next to the app folder
var kyeRingDir = new DirectoryInfo(keyRingPath);
builder.Services.AddDataProtection().PersistKeysToFileSystem(kyeRingDir);


var app = builder.Build();

if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
