using Infrastructure.Validation.Business;
using Infrastructure.Validation.Business.Model;
using Infrastructure.Validation.FluentValidator.Adapters.Model;

namespace Infrastructure.Validation.FluentValidator.Adapters;


public class CommonAdapter<TRequest>(
    ICommonClient<TRequest> client) : IValidator<TRequest> {

    public async Task<List<Error>> Validate(TRequest request, CancellationToken token) {
        var techModel = await client.Validate(request, token);
        var businessModel = techModel.Select(ToBusiness).ToList();
        return businessModel;
    }

    private static Error ToBusiness(Model.Model model) => new(model.Name, model.Message);
}

