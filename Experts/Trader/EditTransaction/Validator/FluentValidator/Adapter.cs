using Common.Validation.Business.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.EditTransaction.Validator.FluentValidator;

public class Adapter(Adapter.IClient client) : Service.IValidator {
    public async Task<List<Error>> Validate(Service.Request request, CancellationToken token) {
        var clientModel = await client.Validate(request, token);
        var businessModel = clientModel.Select(ToBusiness).ToList();
        return businessModel;
        static Error ToBusiness(ClientModel model) => new(model.Name, model.Message);
    }

    public record ClientModel(string Name, string Message);

    public interface IClient {
        Task<List<ClientModel>> Validate(Service.Request request, CancellationToken token);
    }
}


public static class AdapterExtensions {
    public static IServiceCollection AddValidatorAdapter(this IServiceCollection services) => services
        .AddScoped<Service.IValidator, Adapter>()
        .AddValidatorClient();
}