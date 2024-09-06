using Common.Valdation.Adapters.Fluentvalidation;
using Common.Valdation.Adapters.Fluentvalidation.Model;
using FluentValidation;
using FluentValidation.Results;

namespace Common.Valdation.Technology.FluentValidation;

public class ValidatorClient<TRequest>(IValidator<TRequest> validator) : IValidatorClient<TRequest>
{

    public async Task<List<ErrorModel>> Validate(TRequest request, CancellationToken token)
    {
        var techData = await validator.ValidateAsync(request, token);
        var techModel = techData.Errors.Select(ToModel).ToList();
        return techModel;
    }

    private static ErrorModel ToModel(ValidationFailure data) => new ErrorModel()
    {
        Name = data.PropertyName,
        Message = data.ErrorMessage
    };
}
 