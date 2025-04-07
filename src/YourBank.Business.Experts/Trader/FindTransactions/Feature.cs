using System.ComponentModel;
using Business.Domain;
using Infrastructure.Adapters.Blazor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Business.Experts.Trader.FindTransactions;

public interface IFindTransactionsAdapter {
    Task<ViewModel> Execute(string name, string userId, CancellationToken token);
}
public record ViewModel {
    public MetaVM Meta { get; set; }
    public List<ErrorVM> Errors { get; set; } = [];
    public DataListModel<TransactionVM> Transactions { get; set; }
    public class MetaVM {
        public Guid Id { get; internal set; }
    }

    public class ErrorVM {
        public string Name { get; internal set; }
        public string Message { get; internal set; }
    }

    public record TransactionVM {
        [DisplayName("ID")]
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
public class FeatureAdapter(IFindTransactions service) : IFindTransactionsAdapter {
    public async Task<ViewModel> Execute(string name, string userId, CancellationToken token) {
       
        var request = new FindTransactionsRequest {
            Name = name,
            UserId = userId
        };

        token.ThrowIfCancellationRequested();

        var response = await service.Execute(request, token);

        var viewModel = new ViewModel();

        viewModel.Meta = ToMetaVM(response.Request);
        viewModel.Errors = response.Errors.Select(ToErrorVM).ToList();
        viewModel.Transactions = new();
        viewModel.Transactions.Rows = response.Transactions.Select(ToTranasactionVM).ToList();
        viewModel.Transactions.Columns.Add(x => x.Id);
        viewModel.Transactions.Columns.Add(x => x.Name);

        token.ThrowIfCancellationRequested();

        return viewModel;

        static ViewModel.MetaVM ToMetaVM(FindTransactionsRequest x) =>
            new() { Id = x.Id, };

        static ViewModel.TransactionVM ToTranasactionVM(Trade x) =>
            new() { Id = x.Id, Name = x.Name };

        static ViewModel.ErrorVM ToErrorVM(Domain.Error x) =>
            new() { Name = x.Name, Message = x.Message };
    }
}


public interface IFindTransactions {
    Task<FindTransactionsResponse> Execute(FindTransactionsRequest request, CancellationToken token);
}
public class FindTransactionsRequest {
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; }
    public string UserId { get; set; }
}
public class FindTransactionsResponse {
    public Guid Id { get; set; } = Guid.NewGuid();
    public bool Enabled { get; set; } = false;
    public DateTime? CompletedAt { get; set; }
    public DateTime? FailedAt { get; set; }
    public Exception? Exception { get; set; }
    public FindTransactionsRequest Request { get; set; }

    public List<Error> Errors { get; set; } = [];
    public List<Trade> Transactions { get; set; } = [];
    public DateTime StartedAt { get; internal set; }
}
internal class Feature(
    IValidatorAdapter validator,
    IRepositoryAdapter repository,
    IClockAdapter clock,
    IOptionsSnapshot<Settings> settings) : IFindTransactions {

    public async Task<FindTransactionsResponse> Execute(FindTransactionsRequest request, CancellationToken token) {
        var response = new FindTransactionsResponse();
        response.Request = request;

        try {
            response.StartedAt = clock.GetTime();

            response.Errors = await validator.Validate(request, settings.Value, token);
            if (response.Errors.Any())
                return response;

            response.Transactions = await repository.Find(request, token);

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

    public static IServiceCollection AddFindTransactions(this IServiceCollection services,
        ConfigurationManager config) {

        services.Configure<Settings>(config.GetSection("Features:FindTransactions"));

        return services
            .AddScoped<Feature>()
            .AddValidator()
            .AddRepository()
            .AddClock();
    }
}
