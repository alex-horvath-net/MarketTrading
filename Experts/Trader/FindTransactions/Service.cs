using Common.Business.Model;
using Common.Validation.Business.Model;
using Experts.Trader.FindTransactions.Clock.Microsoft;
using Experts.Trader.FindTransactions.Flag.Microsoft;
using Experts.Trader.FindTransactions.Repository.EntityFramework;
using Experts.Trader.FindTransactions.Validator.FluentValidator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions;

public class Service(Service.IValidator validator, Service.IFlag flag, Service.IRepository repository, Service.IClock clock) {
  
    public async Task<Response> Execute(Request request, CancellationToken token) {
        var response = new Response();
        try {
            response.Errors = await validator.Validate(request, token);
            if (response.Errors.Count > 0)
                return response;

            response.IsPublic = flag.IsPublic(request, token);

            response.Request = request;

            response.Transactions = await repository.FindTransactions(request, token);

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
        public bool IsPublic { get; set; } = false;
        public DateTime? StopedAt { get; set; }
        public DateTime? FailedAt { get; internal set; }
        public Exception? Exception { get; set; }
        public Request? Request { get; set; }
        public List<Error> Errors { get; set; } = [];
        public List<Transaction> Transactions { get; set; } = [];
    }

    public interface IValidator { Task<List<Error>> Validate(Request request, CancellationToken token); }

    public interface IRepository { Task<List<Transaction>> FindTransactions(Service.Request request, CancellationToken token); }

    public interface IFlag { bool IsPublic(Service.Request request, CancellationToken token); }

    public interface IClock { DateTime GetTime(); }
}

public static class ServiceExtensions {

    public static IServiceCollection AddFindTransactions(this IServiceCollection services, ConfigurationManager configuration) => services
        .AddScoped<Service>()
        .AddFlagAdapter()
        .AddClockAdapter()
        .AddValidatorAdapter()
        .AddRepositoryAdapter(configuration);
}