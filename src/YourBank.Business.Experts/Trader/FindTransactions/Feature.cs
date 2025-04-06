using Business.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Business.Experts.Trader.FindTransactions;

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
internal class Settings {
    public bool Enabled { get; set; } = false;
}

internal interface IValidatorAdapter { Task<List<Domain.Error>> Validate(FindTransactionsRequest request, Settings settings, CancellationToken token); }
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
