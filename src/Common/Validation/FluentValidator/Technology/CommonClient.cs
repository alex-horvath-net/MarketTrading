using Infrastructure.Validation.FluentValidator.Adapters.Model;

namespace Infrastructure.Validation.FluentValidator.Technology;

public class CommonClient<TRequest>(FluentValidation.IValidator<TRequest> validator) :  Adapters.ICommonClient<TRequest> {

    public async Task<List<Model>> Validate(TRequest request, CancellationToken token) {
        var tech = await validator.ValidateAsync(request, token);
        var techModel = tech.Errors.Select(ToModel).ToList();
        return techModel;
    }

    private static Model ToModel(FluentValidation.Results.ValidationFailure data) => new(data.PropertyName, data.ErrorMessage);
}
