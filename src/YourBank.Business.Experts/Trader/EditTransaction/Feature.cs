using Business.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Experts.Trader.EditTransaction;

public class Feature(
    Feature.IValidate validate, 
    Feature.IEdit edit) {
    public async Task<Response> Execute(Request request, CancellationToken token) {
        var response = new Response(request, token);

        if (await validate.Run(response))
            return response;

        await edit.Run(response);

        return response;
    }

    public class Request {
        public Guid Id { get; set; } = Guid.NewGuid();
        public long TransactionId { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
    }

    public class Response {
        public Response(Request request, CancellationToken token) {
            Request = request;
            Token = token;
        }
        public Guid Id { get; } = Guid.NewGuid();
        public bool IsPublic { get; set; } = false;
        public DateTime? StopedAt { get; set; }
        public DateTime? FailedAt { get; internal set; }
        public Exception? Exception { get; set; }
        public List<Error> Errors { get; set; } = [];
        public Trade Transaction { get; set; }
        public Request Request { get; }
        public CancellationToken Token { get; }
    }

    public interface IValidate { Task<bool> Run(Feature.Response response); }

    public interface IEdit { Task<bool> Run(Feature.Response response); }
}

public static class FeatureExtensions {
    public static IServiceCollection AddEditTransaction(this IServiceCollection services, ConfigurationManager config) => services
        .AddScoped<Feature>()
        .AddValidate()
        .AddEdit();
}
