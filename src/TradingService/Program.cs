using Domain;
using TradingService;
using TradingService.Models;
using TradingService.PlaceTrade;
using TradingService.Services;

var builder = WebApplication.CreateBuilder(args);

// Domain service registration
builder.Services.AddScoped<Trade>();
builder.Services.AddSingleton<IRiskService, RiskService>();
builder.Services.AddSingleton<IComplianceService, ComplianceService>();
builder.Services.AddSingleton<OrderService>();

var app = builder.Build();

// Minimal API: Place new order
app.MapGet("/ping", () => "pong");
app.MapPlaceTrade();

app.Run();
