using System.ComponentModel;
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

    public FindTradesRequest ToRequest() => new(Id: Guid.NewGuid(), Issuer, TraderId, Instrument, Side, FromDate, ToDate);

    //private void ApplyFilters() {
    //    filteredTrades = allTrades
    //        .Where(t => string.IsNullOrWhiteSpace(filterInstrument) || t.Instrument.Contains(filterInstrument, StringComparison.OrdinalIgnoreCase))
    //        .Where(t => string.IsNullOrWhiteSpace(filterSide) || t.Side.ToString() == filterSide)
    //        .Where(t => !filterFromDate.HasValue || t.SubmittedAt >= filterFromDate)
    //        .Where(t => !filterToDate.HasValue || t.SubmittedAt <= filterToDate.Value.Date.AddDays(1).AddTicks(-1))
    //        .ToList();
    //}
}
public record FindTradesViewModel : ViewModel {
    public TableModel<Trade> Trades { get; set; } = new();
    public int TotalCount { get; set; }
    public int BuyCount { get; set; }
    public int SellCount { get; set; }


    internal static FindTradesViewModel From(FindTradesResponse response) {
        var viewModel = new FindTradesViewModel();
        viewModel.Meta = ToMetaVM(response.Request);
        viewModel.Errors = response.Errors.Select(ToErrorVM).ToList();
        viewModel.Trades.Rows = response.Trades;
        viewModel.Trades.Columns.Add(x => x.Id);
        viewModel.Trades.Columns.Add(x => x.Instrument);
        viewModel.Trades.Columns.Add(x => x.Side);
        viewModel.Trades.Columns.Add(x => x.Quantity);
        viewModel.Trades.Columns.Add(x => x.GetType);
        viewModel.Trades.Columns.Add(x => x.Price);
        viewModel.Trades.Columns.Add(x => x.TimeInForce);
        viewModel.TotalCount = response.Trades.Count();
        viewModel.BuyCount = response.Trades.Count(trade => trade.Side == TradeSide.Buy);
        viewModel.SellCount = response.Trades.Count(trade => trade.Side == TradeSide.Sell);
        return viewModel;

        static MetaVM ToMetaVM(FindTradesRequest x) =>
            new() { Id = x.Id, };


        static ErrorVM ToErrorVM(Error x) =>
            new() { Name = x.Name, Message = x.Message };

    }
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
        var request = InputModel.ToRequest();
        var response = await feature.Execute(request, token);
        ViewModel = FindTradesViewModel.From(response);
    }
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
