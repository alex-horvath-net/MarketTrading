using Business.Domain;
using Business.Experts.Trader.EditTransaction;
using Infrastructure.Validation.Business.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Experts.Trader.FindTransactions;

public interface IFeature { Task<Feature.Response> Execute(Feature.Request request, CancellationToken token); }
public class Feature(
    Feature.ICheck check,
    Feature.IValidate validate,
    Feature.IRepository repository,
    Feature.IClock clock) : IFeature {

    public async Task<Response> Execute(Request request, CancellationToken token) {
        var response = new Response(request, token);

        try {
            if (check.Run(response))
                return response;

            if (await validate.Run(response))
                return response;

            response.Transactions = await repository.FindTransactions(request, token);

            token.ThrowIfCancellationRequested();
        } catch (Exception ex) {
            response.FailedAt = clock.GetTime();
            response.Exception = ex;
            throw;
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

        public Response(Request request, CancellationToken token) {
            Request = request;
            Token = token;
        }
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool Enabled { get; set; } = false;
        public DateTime? StopedAt { get; set; }
        public DateTime? FailedAt { get; set; }
        public Exception? Exception { get; set; }
        public Request Request { get; }
        public CancellationToken Token { get; }

        public List<Error> Errors { get; set; } = [];
        public List<Trade> Transactions { get; set; } = [];
    }

    public interface ICheck { bool Run(Response response); }
    public interface IValidate { Task<bool> Run(Response response); }

    public interface IClock { DateTime GetTime(); }


    public interface IRepository { Task<List<Trade>> FindTransactions(Request request, CancellationToken token); }





}

public static class FeatureExtensions {

    public static IServiceCollection AddFindTransactions(this IServiceCollection services, ConfigurationManager config) => services
        .AddScoped<Feature>()
        .AddValidator()
        .AddRepository()
        .AddFlag()
        .AddClock();
}


