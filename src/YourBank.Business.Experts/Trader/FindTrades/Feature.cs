using System.ComponentModel;
using Business.Domain;
using Business.Experts.Trader.FindTransactions;
using Infrastructure.Adapters.Blazor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Business.Experts.Trader.FindTrades;

public record FindTradesInputModel() :InputModel {

    public string? Instrument { get; set; }
    public string? Side { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }

    public FindTransactionsRequest ToRequest() => new() {
        Id = Guid.NewGuid(),
        Issuer = "TradingPortal",
        TransactionName = UserName,
        UserId = UserId,
    };

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
    

    public record TransactionVM {
        [DisplayName("ID")]
        public string Id { get; set; }
        public string Name { get; set; }
    }

    internal static FindTradesViewModel From(FindTransactionsResponse response) {
        var viewModel = new FindTradesViewModel();
        viewModel.Meta = ToMetaVM(response.Request);
        viewModel.Errors = response.Errors.Select(ToErrorVM).ToList();
        viewModel.Trades.Rows = response.Trades.Select(ToTranasactionVM).ToList();
        viewModel.Trades.Columns.Add(x => x.Id);
        viewModel.Trades.Columns.Add(x => x.Name);

        return viewModel;

        static MetaVM ToMetaVM(FindTransactionsRequest x) =>
            new() { Id = x.Id, };

        static TransactionVM ToTranasactionVM(Trade x) =>
            new() { Id = x.TraderId, Name = x.Instrument };

        static ErrorVM ToErrorVM(Error x) =>
            new() { Name = x.Name, Message = x.Message };

    }
}


public interface IFeatureAdapter {
    Task<FindTradesViewModel> Execute(FindTradesInputModel input, CancellationToken token);
}

internal class FeatureAdapter(IFeature feature) : IFeatureAdapter {
    // Blazor should be abel to call this adapter with minimum effort and zero technology leaking
    public async Task<FindTradesViewModel> Execute(FindTradesInputModel input, CancellationToken token) {
        var request = input.ToRequest();
        var response = await feature.Execute(request, token);
        var viewModel = FindTradesViewModel.From(response);
        return viewModel;
    }
}


internal interface IFeature {
    Task<FindTransactionsResponse> Execute(FindTransactionsRequest request, CancellationToken token);
}
public class FindTransactionsRequest {
    public Guid Id { get; set; }
    public string? TransactionName { get; set; }
    public string UserId { get; set; }
    public string Issuer { get; internal set; }
}
internal class FindTransactionsResponse {
    public Guid Id { get; set; } = Guid.NewGuid();
    public bool Enabled { get; set; } = false;
    public DateTime? CompletedAt { get; set; }
    public DateTime? FailedAt { get; set; }
    public Exception? Exception { get; set; }
    public FindTransactionsRequest Request { get; set; }

    public List<Error> Errors { get; set; } = [];
    public List<Trade> Trades { get; set; } = [];
    public DateTime StartedAt { get; internal set; }
}
internal class Feature(
    IValidatorAdapter validator,
    IRepositoryAdapter repository,
    IClockAdapter clock,
    IOptionsSnapshot<Settings> settings) : IFeature {

    public async Task<FindTransactionsResponse> Execute(FindTransactionsRequest request, CancellationToken token) {
        var response = new FindTransactionsResponse();
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

internal interface IValidatorAdapter { Task<List<Error>> Validate(FindTransactionsRequest request, Settings settings, CancellationToken token); }
internal interface IClockAdapter { DateTime GetTime(); }
internal interface IRepositoryAdapter { Task<List<Trade>> Find(FindTransactionsRequest request, CancellationToken token); }

public static class FeatureExtensions {

    public static IServiceCollection AddFindTrade(this IServiceCollection services,        ConfigurationManager config) {

        services.Configure<Settings>(config.GetSection("Features:FindTrades"));

        return services
            .AddScoped<IFeatureAdapter, FeatureAdapter>()
            .AddScoped<IFeature, Feature>()
            .AddValidator()
            .AddRepository(config)
            .AddClock();
    }
}
