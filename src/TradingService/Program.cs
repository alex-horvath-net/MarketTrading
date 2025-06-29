using TradingService.Features.FindTrades;
using TradingService.Features.PlaceTrade;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var env = builder.Environment;

builder.Services.AddFindTrade(config);
builder.Services.AddPlaceTrade(config);

var app = builder.Build();

app.MapGet("/ping", () => "pong");
app.MapPlaceTrade();

app.Run();
