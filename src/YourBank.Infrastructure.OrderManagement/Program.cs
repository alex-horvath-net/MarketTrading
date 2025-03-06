using YourBank.Business.Domain;
using YourBank.Infrastructure.OrderManagement.Models;
using YourBank.Infrastructure.OrderManagement.Services;
// Manages trade orders by handling order placement, cancellation, and tracking execution statuses.
// It integrates with external exchange APIs (e.g., via FIX).

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<Trade>();
// Register our services as singletons.
builder.Services.AddSingleton<IRiskService, RiskService>();
builder.Services.AddSingleton<IComplianceService, ComplianceService>();
builder.Services.AddSingleton<OrderService>();

var app = builder.Build();

// Minimal API endpoint for order placement.
// Example POST request to /orders with a JSON body:
// {
//   "symbol": "AAPL",
//   "orderType": "Market",
//   "quantity": 100
// }
app.MapPost("/orders", (Order order, OrderService orderService) => {
    var response = orderService.PlaceOrder(order);
    return Results.Ok(response);
});

app.Run();
