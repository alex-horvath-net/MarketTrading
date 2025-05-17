using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Business.Domain;
using Infrastructure.Adapters.Blazor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using static Business.Experts.Trader.PlaceTrade.PlaceTradeViewModel;

namespace Business.Experts.Trader.PlaceTrade;

public interface IFeatureAdapter {
    PlaceTradeInputModel InputModel { get; set; }
    PlaceTradeViewModel ViewModel { get; set; }
    Task Execute(CancellationToken token);
}

internal class Adapter(IFeature feature, ILogger<Adapter> logger) : IFeatureAdapter {
    public PlaceTradeInputModel InputModel { get; set; }
    public PlaceTradeViewModel ViewModel { get; set; }
    // Blazor UI should be abel to call this adapter with no effort or leaking any UI technology 
    public async Task Execute(CancellationToken token) {

        var request = ToRequest();
        var response = await feature.Execute(request, token);
        ViewModel =
            response.Exception == null ?
            FromResponse(response) :
            FromException(response.Exception);
    }

    private PlaceTradeRequest ToRequest() => new(
        Id: InputModel.Id ?? Guid.NewGuid(),
        Issuer: InputModel.Issuer,
        TraderId: InputModel.TraderId,
        Instrument: InputModel.Instrument,
        Side: InputModel.Side,
        Quantity: InputModel.Quantity,
        Price: InputModel.Price,
        OrderType: InputModel.OrderType,
        TimeInForce: InputModel.TimeInForce,
        StrategyCode: InputModel.StrategyCode,
        PortfolioCode: InputModel.PortfolioCode,
        UserComment: InputModel.UserComment,
        ExecutionRequestedForUtc: InputModel.ExecutionRequestedFor
    );

    private PlaceTradeViewModel FromResponse(PlaceTradeResponse response) {
        var viewModel = new PlaceTradeViewModel();
        viewModel.Meta = ToMetaVM(response.Request);
        viewModel.Errors = response.Errors.Select(ToErrorVM).ToList();
        viewModel.Trade = ToTradeVM(response.Trade);
        viewModel.Result = $"✅ Trade submitted: {response.Trade.Id}";
        viewModel.AlertCssClass = "alert-success";

        return viewModel;

        MetaVM ToMetaVM(PlaceTradeRequest x) =>
            new() { Id = x.Id };

        TradeVM ToTradeVM(Trade x) =>
            new() { Id = x.TraderId, Name = x.Instrument };

        ErrorVM ToErrorVM(Error error) =>
            new() { Name = error.Name, Message = error.Message };
    }

    private PlaceTradeViewModel FromException(Exception ex) {
        var viewModel = new PlaceTradeViewModel();
        viewModel.Errors = [ToErrorVM(ex)];
        viewModel.Result = $"❌ {ex.Message}";
        viewModel.AlertCssClass = "alert-danger";

        return viewModel;

        ErrorVM ToErrorVM(Exception exception) =>
            new() { Name = "", Message = exception.Message };

    }
}

public record PlaceTradeInputModel(string TraderId) : InputModel(TraderId) {

    [Required]
    public string Instrument { get; set; } = "";
    [Range(1, int.MaxValue)]
    public decimal Quantity { get; set; }
    public Domain.TradeSide Side { get; set; } = Domain.TradeSide.Buy;
    public Domain.OrderType OrderType { get; set; } = Domain.OrderType.Market;
    public bool OrderTypeIsNotMarket => OrderType != Domain.OrderType.Market;
    [Range(0.01, double.MaxValue)]
    public decimal? Price { get; set; }
    public Domain.TimeInForce TimeInForce { get; set; } = Domain.TimeInForce.Day;
    public string? StrategyCode { get; set; }
    public string? PortfolioCode { get; set; }
    public string? UserComment { get; set; }
    public DateTime? ExecutionRequestedFor { get; set; }
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
}


public static class Extensions {

    public static IServiceCollection AddPlaceTrade(this IServiceCollection services, ConfigurationManager config) {

        services.Configure<Settings>(config.GetSection("Features:PlaceTrade"));

        return services
            .AddScoped<IFeatureAdapter, Adapter>()
            .AddFeature(config);
    }
}
