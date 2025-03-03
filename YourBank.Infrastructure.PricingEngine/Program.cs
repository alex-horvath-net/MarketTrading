// Aggregates and processes real‑time market data (from providers like Bloomberg or Refinitiv) to compute final price quotes.

using YourBank.Infrastructure.PricingEngine.Services;

var builder = WebApplication.CreateBuilder(args);

// Register HttpClient (used by the Bloomberg provider)
builder.Services.AddHttpClient();

// Register our custom services for market data, risk metrics, and price calculation
builder.Services.AddScoped<IMarketDataProvider, BloombergMarketDataProvider>();
builder.Services.AddScoped<IRiskMetricsService, RiskMetricsService>();
builder.Services.AddScoped<IPriceCalculator, BlackScholesPriceCalculator>();

var app = builder.Build();

// Define a minimal API endpoint to calculate a theoretical price.
// Example usage: GET /price?symbol=AAPL&strike=120&timeToExpiry=0.5
app.MapGet("/price", async (string symbol, decimal strike, double timeToExpiry, IPriceCalculator calculator) => {
    var price = await calculator.CalculatePriceAsync(symbol, strike, timeToExpiry);
    return Results.Ok(new { Symbol = symbol, Strike = strike, TimeToExpiry = timeToExpiry, Price = price });
});

app.Run();
