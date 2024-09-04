using FluentValidation;

namespace Experts.Trader.ReadTransactions.Validate;

public class Validator(IValidator<Request> validator) : IValidator
{
    public async Task<List<string>> Validate(Request request, CancellationToken token)
    {
        var validationResult = await validator.ValidateAsync(request, token);
        return validationResult.Errors.Select(e => e.ErrorMessage).ToList();
    }

    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {

            RuleFor(request => request)
                .NotNull().WithMessage(ErrorMesages.RequestNull);

            RuleFor(request => request.Name)
                .MinimumLength(3).When(request => !string.IsNullOrEmpty(request.Name)).WithMessage(ErrorMesages.NameShort);
        }
    }

    public static class ErrorMesages
    {
        public static string RequestNull => "Request must be provided.";
        public static string NameShort => "Name must be at least 3 characters long if provided.";
    }
}

