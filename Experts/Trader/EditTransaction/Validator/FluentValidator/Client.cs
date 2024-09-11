using FluentValidation;

namespace Experts.Trader.EditTransaction.Validator.FluentValidator;

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
