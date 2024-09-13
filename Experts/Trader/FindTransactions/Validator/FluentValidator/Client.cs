using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions.Validator.FluentValidator;

public class Client(IValidator<Service.Request> core) : Adapter.IClient
{
    public async Task<List<Adapter.ClientModel>> Validate(Service.Request request, CancellationToken token)
    {
        var techModel = await core.ValidateAsync(request, token);
        var clientModel = techModel.Errors.Select(ToModel).ToList();
        return clientModel;
    }

    private static Adapter.ClientModel ToModel(FluentValidation.Results.ValidationFailure tech) => new(tech.PropertyName, tech.ErrorMessage);
}

public static class ClientExtensions {

    public static IServiceCollection AddValidatorClient(this IServiceCollection services) => services
        .AddScoped<Adapter.IClient, Client>()
        .AddValidatorTechnology();
}
