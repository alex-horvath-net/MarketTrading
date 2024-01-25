using Common;
using Core;
using Experts;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Services.AddRazorPages();

builder.Services
    .AddCore(config)
    .AddCommon(config)
    .AddExperts();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage();
} else {
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
