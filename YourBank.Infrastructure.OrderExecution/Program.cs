using YourBank.Infrastructure.OrderExecution.Models;
using YourBank.Infrastructure.OrderExecution.Services;

var builder = WebApplication.CreateBuilder(args);

// Register services for dependency injection.
builder.Services.AddSingleton<IOrderExecutionService, OrderExecutionService>();

var app = builder.Build();

// Minimal API endpoint for processing order execution reports.
// Example POST request body (JSON):
// {
//   "orderId": "GUID",
//   "symbol": "AAPL",
//   "executedQuantity": 100,
//   "executionPrice": 135.50,
//   "executionTime": "2023-03-01T15:30:00Z"
// }
app.MapPost("/executeOrder", async (OrderExecution execution, IOrderExecutionService executionService) => {
    var response = await executionService.ProcessExecutionAsync(execution);
    return Results.Ok(response);
});

app.Run();
