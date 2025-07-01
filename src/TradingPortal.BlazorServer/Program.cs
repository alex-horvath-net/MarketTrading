using Microsoft.AspNetCore.DataProtection;
using TradingPortal.BlazorServer.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();


var keyRingPath = Path.Combine(builder.Environment.ContentRootPath, "..", "keys"); // nest to app folder put the kes into the keys folder
builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(keyRingPath));


var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
