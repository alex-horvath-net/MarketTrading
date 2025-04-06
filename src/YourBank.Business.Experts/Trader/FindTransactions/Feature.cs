using Business.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Business.Experts.Trader.FindTransactions;

internal class Feature(
    IValidatorAdapter validator,
    IRepositoryAdapter repository,
    IClockAdapter clock,
    IOptionsSnapshot<Settings> settings) : IFeature {

    public async Task<Response> Execute(
        Request request,
        CancellationToken token) {
        var response = new Response();
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

public interface IFeature {
    Task<Response> Execute(Request request, CancellationToken token);
}
public class Request {
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; }
    public string UserId { get; set; }
}
public class Response {
    public Guid Id { get; set; } = Guid.NewGuid();
    public bool Enabled { get; set; } = false;
    public DateTime? CompletedAt { get; set; }
    public DateTime? FailedAt { get; set; }
    public Exception? Exception { get; set; }
    public Request Request { get; set; }

    public List<Error> Errors { get; set; } = [];
    public List<Trade> Transactions { get; set; } = [];
    public DateTime StartedAt { get; internal set; }
}
internal class Settings {
    public bool Enabled { get; set; } = false;
}

internal interface IValidatorAdapter { Task<List<Domain.Error>> Validate(Request request, Settings settings, CancellationToken token); }
internal interface IClockAdapter { DateTime GetTime(); }
internal interface IRepositoryAdapter { Task<List<Trade>> Find(Request request, CancellationToken token); }

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
