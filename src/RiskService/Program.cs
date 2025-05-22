using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Configure strongly-typed settings from appsettings.json
builder.Services.Configure<RiskSettings>(
    builder.Configuration.GetSection("RiskService"));

builder.Services.AddSingleton<RiskEvaluator>();

var app = builder.Build();

app.MapGet("/ping", () => "pong");

// Minimal API endpoint to evaluate risk
// POST /evaluate { "quantity": 500 }
app.MapPost("/evaluate", (OrderInput input, RiskEvaluator evaluator) => {
    var result = evaluator.Evaluate(input.Quantity);
    return result.IsAccepted
        ? Results.Ok(new { Status = "Accepted", Reason = result.Reason })
        : Results.BadRequest(new { Status = "Rejected", Reason = result.Reason });
});

app.Run();

public class OrderInput {
    public int Quantity { get; set; }
}

public class RiskResult {
    public bool IsAccepted { get; set; }
    public string Reason { get; set; } = string.Empty;
}

public class RiskSettings {
    public int MaxOrderQuantity { get; set; } = 1000;
}

public class RiskEvaluator {
    private readonly RiskSettings _settings;

    public RiskEvaluator(IOptions<RiskSettings> options) {
        _settings = options.Value;
    }

    public RiskResult Evaluate(int quantity) {
        if (quantity <= _settings.MaxOrderQuantity) {
            return new RiskResult {
                IsAccepted = true
            };
        } else {
            return new RiskResult {
                IsAccepted = false,
                Reason = $"Order quantity {quantity} exceeds maximum limit of {_settings.MaxOrderQuantity}."
            };
        }
    }
}