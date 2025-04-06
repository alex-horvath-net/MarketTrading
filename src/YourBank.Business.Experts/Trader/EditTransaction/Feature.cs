using Business.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Experts.Trader.EditTransaction;

public interface IEditTransaction {
    Task<EditTransactionResponse> Execute(EditTransactionRequest request, CancellationToken token);
}

public class EditTransactionRequest {
    public Guid Id { get; set; } = Guid.NewGuid();
    public long TransactionId { get; set; }
    public string Name { get; set; }
    public string UserId { get; set; }
}

public class EditTransactionResponse {
    public Guid Id { get; } = Guid.NewGuid();
    public bool IsPublic { get; set; } = false;
    public DateTime? StopedAt { get; set; }
    public DateTime? FailedAt { get; internal set; }
    public Exception? Exception { get; set; }
    public List<Error> Errors { get; set; } = [];
    public Trade Transaction { get; set; }
    public EditTransactionRequest Request { get; set; }
}

internal class Feature(IValidatorAdapter validator, IRepositoryAdapter repository) {
    public async Task<EditTransactionResponse> Execute(EditTransactionRequest request, CancellationToken token) {
        var response = new EditTransactionResponse();
        response.Request = request;

        response.Errors = await validator.Validate(request, token);
        if (response.Errors.Any())
            return response;

        response.Transaction = await repository.Edit(request, token);

        return response;
    }

}

internal interface IValidatorAdapter { Task<List<Error>> Validate(EditTransactionRequest request, CancellationToken token); }

internal interface IRepositoryAdapter { Task<Trade> Edit(EditTransactionRequest request, CancellationToken token); }

public static class FeatureExtensions {
    public static IServiceCollection AddEditTransaction(this IServiceCollection services, ConfigurationManager config) => services
        .AddScoped<Feature>()
        .AddValidator()
        .AddRepository();
}
