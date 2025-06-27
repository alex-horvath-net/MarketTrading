using Microsoft.Extensions.Options;
using TradingService.Domain;
using TradingService.Features.PlaceTrade.WorkSteps;

namespace TradingService.Features.PlaceTrade;

internal interface IFeature {
    Task<PlaceTradeResponse> Execute(PlaceTradeRequest request, CancellationToken token);
}

internal class Feature(
    IValidatorAdapter validator,
    IRepositoryAdapter repository,
    IClockAdapter clock,
    IOptionsSnapshot<Settings> settings) : IFeature {

    public async Task<PlaceTradeResponse> Execute(PlaceTradeRequest request, CancellationToken token) {
        var response = new PlaceTradeResponse();
        response.Request = request;

        try {
            response.StartedAt = clock.GetTime();

            response.Errors = await validator.Validate(request, settings.Value, token);
            if (response.Errors.Any())
                return response;

            response.Trade = await repository.Create(request, token);

            response.CompletedAt = clock.GetTime();

        } catch (Exception ex) {
            response.FailedAt = clock.GetTime();
            response.Exception = ex;
        }

        return response;
    }
}

public record PlaceTradeRequest(
    Guid Id,
    string Issuer,
    string TraderId,
    string Instrument,
    TradeSide Side,
    decimal Quantity,
    decimal? Price,
    OrderType OrderType,
    TimeInForce TimeInForce,
    string? StrategyCode,
    string? PortfolioCode,
    string? UserComment,
    DateTime? ExecutionRequestedForUtc) {
}

public class PlaceTradeResponse {
    public Guid Id { get; set; } = Guid.NewGuid();
    public bool Enabled { get; set; } = false;
    public DateTime? CompletedAt { get; set; }
    public DateTime? FailedAt { get; set; }
    public Exception? Exception { get; set; }
    public PlaceTradeRequest Request { get; set; }

    public List<Error> Errors { get; set; } = [];
    public Trade Trade { get; set; }
    public DateTime StartedAt { get; internal set; }
}

internal class Settings {
    public bool Enabled { get; set; } = false;
}

internal interface IValidatorAdapter { Task<List<Error>> Validate(PlaceTradeRequest request, Settings settings, CancellationToken token); }
internal interface IClockAdapter { DateTime GetTime(); }
internal interface IRepositoryAdapter { Task<Trade> Create(PlaceTradeRequest request, CancellationToken token); }

internal static class FeatureExtensions {

    public static IServiceCollection AddPlaceTrade(this IServiceCollection services, ConfigurationManager config) {

        services.Configure<Settings>(config.GetSection("Features:PlaceTrade"));

        return services
            .AddScoped<IFeature, Feature>()
            .AddValidator()
            .AddRepository(config)
            .AddClock(); 
    }
}
