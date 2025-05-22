using Domain;
using TradingService;
using TradingService.Models;
using TradingService.Services;

var builder = WebApplication.CreateBuilder(args);

// Domain service registration
builder.Services.AddScoped<Trade>();
builder.Services.AddSingleton<IRiskService, RiskService>();
builder.Services.AddSingleton<IComplianceService, ComplianceService>();
builder.Services.AddSingleton<OrderService>();

var app = builder.Build();

// Minimal API: Place new order
app.MapPost("/orders", (Order order, OrderService orderService) => {
    var response = orderService.PlaceOrder(order);
    return Results.Ok(response);
});

app.Run();
