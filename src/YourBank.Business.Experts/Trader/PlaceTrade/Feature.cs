using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Business.Domain;
using Infrastructure.Adapters.Blazor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Business.Experts.Trader.PlaceTrade;
public record PlaceTradeInputModel : InputModel {
    [Required]
    public string Instrument { get; set; } = "";
    [Range(1, int.MaxValue)]
    public decimal Quantity { get; set; }
    public TradeSide Side { get; set; } = TradeSide.Buy;
    public OrderType OrderType { get; set; } = OrderType.Market;
    [Range(0.01, double.MaxValue)]
    public decimal? Price { get; set; }
    public TimeInForce TimeInForce { get; set; } = TimeInForce.Day;
    public string? StrategyCode { get; set; }
    public string? PortfolioCode { get; set; }
    public string? UserComment { get; set; }
    public DateTime? ExecutionRequestedFor { get; set; }

    public PlaceTradeRequest ToRequest() => new(
        Id: Guid.NewGuid(),
        Issuer: this.Issuer,
        TraderId: this.TraderId,
        Instrument: this.Instrument,
        Side: this.Side,
        Quantity: this.Quantity,
        Price: this.Price,
        OrderType: this.OrderType,
        TimeInForce: this.TimeInForce,
        StrategyCode: this.StrategyCode,
        PortfolioCode: this.PortfolioCode,
        UserComment: this.UserComment,
        ExecutionRequestedForUtc: this.ExecutionRequestedFor
    );
}

public record PlaceTradeViewModel : ViewModel {
    public string? Result { get; set; }
    public string AlertCssClass { get; set; } = "alert-info";

    public TradeVM Trade { get; set; }

    public record TradeVM {
        [DisplayName("ID")]
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public static PlaceTradeViewModel From(PlaceTradeResponse response) {
        var viewModel = new PlaceTradeViewModel();
        viewModel.Meta = ToMetaVM(response.Request);
        viewModel.Errors = response.Errors.Select(ToErrorVM).ToList();
        viewModel.Trade = ToTradeVM(response.Trade);
        viewModel.Result = $"✅ Trade submitted: {response.Trade.Id}";
        viewModel.AlertCssClass = "alert-success";

        return viewModel;

        MetaVM ToMetaVM(PlaceTradeRequest x) =>
            new() { Id = x.Id };

        PlaceTradeViewModel.TradeVM ToTradeVM(Trade x) =>
            new() { Id = x.TraderId, Name = x.Instrument };

        ErrorVM ToErrorVM(Error error) =>
            new() { Name = error.Name, Message = error.Message };
    }


    public static PlaceTradeViewModel From(Exception ex) {
        var viewModel = new PlaceTradeViewModel();
        viewModel.Errors = [ToErrorVM(ex)];
        viewModel.Result = $"❌ {ex.Message}";
        viewModel.AlertCssClass = "alert-danger";

        return viewModel;

        ErrorVM ToErrorVM(Exception exception) =>
            new() { Name = "", Message = exception.Message };

    }
}
public interface IFeatureAdapter {
    Task<PlaceTradeViewModel> Execute(PlaceTradeInputModel input, CancellationToken token);
}

internal class FeatureAdapter(IFeature feature, ILogger<FeatureAdapter> logger) : IFeatureAdapter {
    // Blazor UI should be abel to call this adapter with no effort or leaking any UI technology 
    public async Task<PlaceTradeViewModel> Execute(PlaceTradeInputModel input, CancellationToken token) {

        var request = input.ToRequest();
        var response = await feature.Execute(request, token);
        var viewModel =
            response.Exception == null ?
            PlaceTradeViewModel.From(response) :
            PlaceTradeViewModel.From(response.Exception);

        return viewModel;
    }
}


internal interface IFeature {
    Task<PlaceTradeResponse> Execute(PlaceTradeRequest request, CancellationToken token);
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

            response.Trade = (await repository.Find(request, token)).First();

            response.CompletedAt = clock.GetTime();

        } catch (Exception ex) {
            response.FailedAt = clock.GetTime();
            response.Exception = ex;
        }

        return response;
    }
}

internal class Settings {
    public bool Enabled { get; set; } = false;
}

internal interface IValidatorAdapter { Task<List<Error>> Validate(PlaceTradeRequest request, Settings settings, CancellationToken token); }
internal interface IClockAdapter { DateTime GetTime(); }
internal interface IRepositoryAdapter { Task<List<Trade>> Find(PlaceTradeRequest request, CancellationToken token); }

public static class FeatureExtensions {

    public static IServiceCollection AddFindTransactions(this IServiceCollection services, ConfigurationManager config) {

        services.Configure<Settings>(config.GetSection("Features:FindTrades"));

        return services
            .AddScoped<IFeatureAdapter, FeatureAdapter>()
            .AddScoped<IFeature, Feature>()
            .AddValidator()
            .AddRepository(config)
            .AddClock();
    }
}
