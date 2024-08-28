using FluentValidation;

namespace Experts.Trader.ReadTransactions.Validate;

public class ValidatorTechnologyPlugin(IValidator<Request> validator) : ValidatorAdapterPlugin.ValidatorTechnologyPort {
    public async Task<List<string>> Validate(Request request, CancellationToken token) {
        var validationResult = await validator.ValidateAsync(request, token);
        return validationResult.Errors.Select(e => e.ErrorMessage).ToList();

    }

    public class RequestValidator : AbstractValidator<Request> {
        public RequestValidator() {
            RuleFor(request => request.Name)
                .NotNull().WithMessage("Name cannot be null.")
                .NotEmpty().WithMessage("Name cannot be empty.");
        }
    }
}
