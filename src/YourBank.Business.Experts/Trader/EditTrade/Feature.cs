using Business.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Experts.Trader.EditTrade;

public interface IEditTransaction {
    Task<EditTradeResponse> Execute(EditTradeRequest request, CancellationToken token);
}

public class EditTradeRequest {
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TransactionId { get; set; }
    public string Name { get; set; }
    public string UserId { get; set; }
}

public class EditTradeResponse {
    public Guid Id { get; } = Guid.NewGuid();
    public bool IsPublic { get; set; } = false;
    public DateTime? StopedAt { get; set; }
    public DateTime? FailedAt { get; internal set; }
    public Exception? Exception { get; set; }
    public List<Error> Errors { get; set; } = [];
    public Trade Transaction { get; set; }
    public EditTradeRequest Request { get; set; }
}

internal class Feature(IValidatorAdapter validator, IRepositoryAdapter repository): IEditTransaction {
    public async Task<EditTradeResponse> Execute(EditTradeRequest request, CancellationToken token) {
        var response = new EditTradeResponse();
        response.Request = request;

        response.Errors = await validator.Validate(request, token);
        if (response.Errors.Any())
            return response;

        response.Transaction = await repository.Edit(request, token);

        return response;
    }

}

internal interface IValidatorAdapter { Task<List<Error>> Validate(EditTradeRequest request, CancellationToken token); }

internal interface IRepositoryAdapter { Task<Trade> Edit(EditTradeRequest request, CancellationToken token); }

public static class FeatureExtensions {
    public static IServiceCollection AddEditTrade(this IServiceCollection services, ConfigurationManager config) => services
        .AddScoped< IEditTransaction, Feature>()
        .AddValidator()
        .AddRepository();
}
