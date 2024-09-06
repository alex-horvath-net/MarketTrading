using Common.Valdation.Business;
using Common.Valdation.Business.Model;

namespace Common.Valdation.Adapters.Fluentvalidation;

public class ValidatorAdapter<TRequest>(IValidatorClient<TRequest> validatorClient) : IValidatorAdapter<TRequest>
{
    public async Task<List<Error>> Validate(TRequest request, CancellationToken token)
    {
        var techModel = await validatorClient.Validate(request, token);
        var businessModel = techModel.Select(ToBusiness).ToList();
        return businessModel;
    }

    private static Error ToBusiness(FluentvalidationErrorModel model) => new()
    {
        Name = model.Name,
        Message = model.Message
    };
}
