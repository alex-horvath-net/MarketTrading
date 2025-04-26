using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq.Expressions;
using Business.Domain;
using Infrastructure.Adapters.Blazor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Business.Experts.Trader.FindTrades;

public record FindTradesInputModel(string TraderId) : InputModel(TraderId) {
    public string? Instrument { get; set; }
    public string? Side { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
public record FindTradesViewModel : ViewModel {
    public TableModel<Trade> Trades { get; set; } = new();
    public int TotalCount { get; set; }
    public int BuyCount { get; set; }
    public int SellCount { get; set; }
}

public interface IFeatureAdapter {
    FindTradesInputModel InputModel { get; set; }
    FindTradesViewModel ViewModel { get; set; }
    Task Execute(CancellationToken token);
}

internal class FeatureAdapter(IFeature feature) : IFeatureAdapter {
    public FindTradesInputModel InputModel { get; set; }
    public FindTradesViewModel ViewModel { get; set; }
    // Blazor should be abel to call this adapter with minimum effort and zero technology leaking
    public async Task Execute(CancellationToken token) {
        var request = ToRequest(InputModel);
        var response = await feature.Execute(request, token);
        ViewModel = ToViewModel(response);
    }

    public FindTradesRequest ToRequest(FindTradesInputModel inputModel) => new(
        Id: Guid.NewGuid(),
        inputModel.Issuer,
        inputModel.TraderId,
        inputModel.Instrument,
        inputModel.Side,
        inputModel.FromDate,
        inputModel.ToDate);

    internal static FindTradesViewModel ToViewModel(FindTradesResponse response) => new FindTradesViewModel() {
        Meta = new MetaVM() {
            Id = response.Request.Id
        },
        Errors = response.Errors.Select(x=> new ErrorVM() {
            Name = x.Name,
            Message = x.Message
        }),
        Trades = new TableModel<Trade>() {
            Rows = response.Trades,
            Columns = [
                trade => trade.Instrument,
                trade => trade.Side,
                trade => trade.SubmittedAt,
                trade => trade.Status
            ]
        },
        TotalCount = response.Trades.Count(),
        BuyCount = response.Trades.Count(trade => trade.Side == TradeSide.Buy),
        SellCount = response.Trades.Count(trade => trade.Side == TradeSide.Sell),
    };
}



internal interface IFeature {
    Task<FindTradesResponse> Execute(FindTradesRequest request, CancellationToken token);
}
public record FindTradesRequest(
    Guid Id,
    string Issuer,
    string TraderId,
    string? Instrument,
    string? Side,
    DateTime? fromDate,
    DateTime? ToDate);
internal class FindTradesResponse {
    public Guid Id { get; set; } = Guid.NewGuid();
    public bool Enabled { get; set; } = false;
    public DateTime? CompletedAt { get; set; }
    public DateTime? FailedAt { get; set; }
    public Exception? Exception { get; set; }
    public FindTradesRequest Request { get; set; }

    public List<Error> Errors { get; set; } = [];
    public List<Trade> Trades { get; set; } = [];
    public DateTime StartedAt { get; internal set; }
}
internal class Feature(
    IValidatorAdapter validator,
    IRepositoryAdapter repository,
    IClockAdapter clock,
    IOptionsSnapshot<Settings> settings) : IFeature {

    public async Task<FindTradesResponse> Execute(FindTradesRequest request, CancellationToken token) {
        var response = new FindTradesResponse();
        response.Request = request;

        try {
            response.StartedAt = clock.GetTime();

            response.Errors = await validator.Validate(request, settings.Value, token);
            if (response.Errors.Any())
                return response;

            response.Trades = await repository.Find(request, token);

            response.CompletedAt = clock.GetTime();

        } catch (Exception ex) {
            response.FailedAt = clock.GetTime();
            response.Exception = ex;
            throw;
        }

        return response;
    }
}

internal class Settings {
    public bool Enabled { get; set; } = false;
}

internal interface IValidatorAdapter { Task<List<Error>> Validate(FindTradesRequest request, Settings settings, CancellationToken token); }
internal interface IClockAdapter { DateTime GetTime(); }
internal interface IRepositoryAdapter { Task<List<Trade>> Find(FindTradesRequest request, CancellationToken token); }

public static class FeatureExtensions {

    public static IServiceCollection AddFindTrade(this IServiceCollection services, ConfigurationManager config) {

        services.Configure<Settings>(config.GetSection("Features:FindTrades"));

        return services
            .AddScoped<IFeatureAdapter, FeatureAdapter>()
            .AddScoped<IFeature, Feature>()
            .AddValidator()
            .AddRepository(config)
            .AddClock();
    }
}
