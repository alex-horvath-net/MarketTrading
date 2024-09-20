using Common;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.EditTransaction.Validator.FluentValidator;

public class Client(IValidator<Service.Request> validator) : Adapter.IClient {

    public async Task<List<Adapter.AdapterIssue>> Validate(Service.Request request, CancellationToken token) {
        var technologyIssues = await validator.ValidateAsync(request, token);
        var adapterIssues = technologyIssues.Errors.Select(ToAdapterIssue).ToList();
        return adapterIssues;
    }

    private static Adapter.AdapterIssue ToAdapterIssue(FluentValidation.Results.ValidationFailure tech) => new(tech.PropertyName, tech.ErrorMessage);
}


public static class ClientExtensions {
    public static IServiceCollection AddValidatorClient(this IServiceCollection services) => services
        .AddScoped<Adapter.IClient, Client>()
        .AddValidatorTechnology();


}


