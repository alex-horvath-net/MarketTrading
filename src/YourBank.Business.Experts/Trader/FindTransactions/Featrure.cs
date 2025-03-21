using Business.Domain;
using Business.Experts.Trader.EditTransaction;
using Infrastructure.Validation.Business.Model;
using Microsoft.Extensions.Configuration;
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

    public static IServiceCollection AddFindTransactions(this IServiceCollection services, ConfigurationManager config) => services
        .AddScoped<Feature>()
        .AddValidator()
        .AddRepository()
        .AddFlag()
        .AddClock();
}


