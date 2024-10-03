using Common.Business.Model;
using Common.Validation.Business.Model;
using Experts.Trader.EditTransaction.WorkSteps;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.EditTransaction;

public class BusinessNeed(
    BusinessNeed.IValidator validator,
    BusinessNeed.IRepository repository) {
    public async Task<Response> Execute(Request request, CancellationToken token) {
        var response = new Response();

        response.Request = request;

        response.Errors = await validator.Validate(request, token);
        if (response.Errors.Count > 0)
            return response;

        response.Transaction = await repository.EditTransaction(request, token);

        return response;
    }

    public class Request {
        public Guid Id { get; set; } = Guid.NewGuid();
        public long TransactionId { get; set; }
        public string Name { get; set; }
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
        public Transaction Transaction { get; set; }
    }

    public interface IValidator { Task<List<Error>> Validate(Request request, CancellationToken token); }

    public interface IRepository { Task<Transaction> EditTransaction(BusinessNeed.Request request, CancellationToken token); }
}

public static class Extensions {
    public static IServiceCollection AddEditTransaction(this IServiceCollection services, ConfigurationManager config) => services
        .AddScoped<BusinessNeed>()
        .AddValidatorAdapter()
        .AddRepositoryAdapter();
}
