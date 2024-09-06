using Common.Valdation.Adapters.Fluentvalidation;
using FluentValidation;
using FluentValidation.Results;

namespace Common.Valdation.Technology.FluentValidation;

public class ValidatorClient<TRequest>(IValidator<TRequest> validator) : IValidatorClient<TRequest>
{

    public async Task<List<FluentvalidationErrorModel>> Validate(TRequest request, CancellationToken token)
    {
        var techData = await validator.ValidateAsync(request, token);
        var techModel = techData.Errors.Select(ToModel).ToList();
        return techModel;
    }

    private static FluentvalidationErrorModel ToModel(ValidationFailure data) => new FluentvalidationErrorModel()
    {
        Name = data.PropertyName,
        Message = data.ErrorMessage
    };
}
 