using Business.Domain;
using Business.Experts.IdentityManager.LocalLogIn;
using Infrastructure.Validation.Business.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Experts.Trader.FindTransactions;

public interface IFeature { Task<Featrure.Response> Execute(Featrure.Request request, CancellationToken token); }
public class Featrure(Featrure.IValidator validator, Featrure.IFlag flag, Featrure.IRepository repository, Featrure.IClock clock) : IFeature {

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
        public List<Trade> Transactions { get; set; } = [];
    }

    public interface IClock { DateTime GetTime(); }

    public interface IFlag { bool IsPublic(Request request, CancellationToken token); }

    public interface IRepository { Task<List<Trade>> FindTransactions(Request request, CancellationToken token); }

    public interface IValidator { Task<List<Error>> Validate(Request request, CancellationToken token); }




}

public static class FeatureExtensions {
    public static IServiceCollection AddFindTransactions(this IServiceCollection services, ConfigurationManager configuration) => services
        .AddScoped<ITrigger, Trigger>()
        .AddService(configuration);

    public static IServiceCollection AddService(this IServiceCollection services, ConfigurationManager configuration) => services
        .AddScoped<IFeature, Featrure>()
        .AddClock()
        .AddFlag()
        .AddValidator()
        .AddRepository();

    public static IServiceCollection AddClock(this IServiceCollection services) => services
        .AddScoped<IClockAdapter, DefaultClockAdapter>()
        .AddScoped<IClock, DefaultClock>();

    public static IServiceCollection AddFlag(this IServiceCollection services) => services
        .AddScoped<IFlag, Flag>()
        .AddScoped<Flag.IClient, Flag.Client>();

    public static IServiceCollection AddRepository(this IServiceCollection services) => services
        .AddScoped<IRepository, Repository>()
        .AddScoped<Repository.IClient, Repository.Client>();


    public static IServiceCollection AddValidator(this IServiceCollection services) => services
        .AddScoped<OutputPorts.IValidator, Adapter>()
        .AddScoped<Adapter.IClient, Adapter.Client>()
        .AddScoped<IValidator<Request>, Adapter.Client.Technology>();
}


