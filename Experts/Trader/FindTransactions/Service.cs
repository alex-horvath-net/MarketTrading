using Common.Business.Model;
using Common.Validation.Business.Model;
using Experts.Trader.FindTransactions.WorkSteps;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions;

public interface IService {
    Task<Response> Execute(Request request, CancellationToken token);
}
public class Request {
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; }
    public string UserId { get; set; }
}

public class Response {
    public Guid Id { get; set; } = Guid.NewGuid();
    public bool IsUnderConstruction { get; set; } = false;
    public DateTime? StopedAt { get; set; }
    public DateTime? FailedAt { get; internal set; }
    public Exception? Exception { get; set; }
    public Request? Request { get; set; }
    public List<Error> Errors { get; set; } = [];
    public List<Transaction> Transactions { get; set; } = [];
}


public class Service(Service.IValidator validator, Service.IFlag flag, Service.IRepository repository, Service.IClock clock) : IService {

    public async Task<Response> Execute(Request request, CancellationToken token) {
        var response = new Response();
        try {
            response.IsUnderConstruction = flag.IsPublic(request, token);
            response.Request = request;
            response.Errors = await validator.Validate(request, token);
            if (response.Errors.Count > 0)
                return response;


            response.Transactions = await repository.FindTransactions(request, token);

            token.ThrowIfCancellationRequested();
        } catch (Exception ex) {
            response.Exception = ex;
        } finally {
            response.StopedAt = clock.GetTime();
        }
        return response;
    }

    public interface IFlag { bool IsPublic(Request request, CancellationToken token); }

    public interface IClock { DateTime GetTime(); }

    public interface IValidator { Task<List<Error>> Validate(Request request, CancellationToken token); }

    public interface IRepository { Task<List<Transaction>> FindTransactions(Request request, CancellationToken token); }
}

public static class ServiceExtensions {

    public static IServiceCollection AddService(this IServiceCollection services, ConfigurationManager configuration) => services
        .AddScoped<IService, Service>()
        .AddClock()
        .AddFlag()
        .AddValidator()
        .AddRepository(configuration);
}