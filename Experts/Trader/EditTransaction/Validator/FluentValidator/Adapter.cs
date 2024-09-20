using Common.Validation.Business.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.EditTransaction.Validator.FluentValidator;

public class Adapter(Adapter.IClient validator) : Service.IValidator {
    public async Task<List<Error>> Validate(Service.Request request, CancellationToken token) {
        var adapterIssues = await validator.Validate(request, token);
        var businessIssues = adapterIssues.Select(ToBusiness).ToList();
        return businessIssues;
        static Error ToBusiness(AdapterIssue model) => new(model.Name, model.Message);
    }

    public record AdapterIssue(string Name, string Message);

    public interface IClient {
        Task<List<AdapterIssue>> Validate(Service.Request request, CancellationToken token);
    }
}


public static class AdapterExtensions {
    public static IServiceCollection AddValidatorAdapter(this IServiceCollection services) => services
        .AddScoped<Service.IValidator, Adapter>()
        .AddValidatorClient();
}