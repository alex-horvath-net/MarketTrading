// Aggregates and processes real‑time market data (from providers like Bloomberg or Refinitiv) to compute final price quotes.

using PricingService.Services;

var builder = WebApplication.CreateBuilder(args);

// Register HttpClient for external market data calls.
builder.Services.AddHttpClient();

// Register our custom services.
builder.Services.AddScoped<IMarketDataProvider, BloombergMarketDataProvider>();
builder.Services.AddScoped<IRiskMetricsService, RiskMetricsService>();
builder.Services.AddScoped<IPriceCalculator, BlackScholesPriceCalculator>();

var app = builder.Build();

// Minimal API endpoint to calculate option price and Greeks.
// Example call: GET /price?symbol=AAPL&strike=120&timeToExpiry=0.5
app.MapGet("/price", async (string symbol, decimal strike, double timeToExpiry, IPriceCalculator calculator) => {
    var result = await calculator.CalculatePriceAsync(symbol, strike, timeToExpiry);
    return Results.Ok(result);
});

app.Run();
