using FluentValidation;

namespace Experts.Account.Register;


public class ValidatorAdapterPlugin(ValidatorAdapterPlugin.ValidatorTechnologyPort validatorTechnologyPort) : Feature.IValidatorAdapterPort {
    public async Task<List<string>> Validate(Feature.Request request, CancellationToken token) {
        var adapterData = await validatorTechnologyPort.Validate(request, token);
        var businessData = adapterData;
        return businessData;
    }

    public interface ValidatorTechnologyPort {
        public Task<List<string>> Validate(Feature.Request request, CancellationToken token);
    }
}


public class ValidatorTechnologyPlugin(IValidator<Feature.Request> validator) : ValidatorAdapterPlugin.ValidatorTechnologyPort {
    public async Task<List<string>> Validate(Feature.Request request, CancellationToken token) {
        var validationResult = await validator.ValidateAsync(request, token);
        return validationResult.Errors.Select(e => e.ErrorMessage).ToList();

    }

    public class RequestValidator : AbstractValidator<Feature.Request> {
        public RequestValidator() {
            RuleFor(request => request.Name)
                .NotNull().WithMessage("Name cannot be null.")
                .NotEmpty().WithMessage("Name cannot be empty.");
        }
    }
}
