using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace TradingService.Features.EditTrade;

public class ValidatorAdapter(RequestValidator requestValidator) : IValidatorAdapter {
    public async Task<List<Domain.Error>> Validate(EditTradeRequest request, CancellationToken token) {
        var issues = await requestValidator.ValidateAsync(request, token);
        var errors = issues.Errors.Select(MakeItFluentValidationFree).ToList();
        return errors;
    }

    private static Domain.Error MakeItFluentValidationFree(FluentValidation.Results.ValidationFailure error) =>
         new(error.PropertyName, error.ErrorMessage);
}

public class RequestValidator : AbstractValidator<EditTradeRequest> {
    public RequestValidator(IRepository repository) {
        RuleFor(x => x)
            .NotNull()
            .WithMessage("Request must be provided.");

        RuleFor(x => x.TransactionId)
            .NotNull()
            .WithMessage("TransactionId must be provided.");

        RuleFor(x => x.TransactionId)
            .MustAsync(async (id, token) => {
                var transaction = await repository.FindById(id, token);
                return transaction != null;
            })
            .WithMessage("Refered TransactionId is not found.");

        RuleFor(x => x.UserId)
            .NotNull()
            .WithMessage("UserId must be provided.");

        RuleFor(x => x.Name)
            .MinimumLength(3)
            .WithMessage("Symbol must be at least 3 characters long.");
        RuleFor(x => x.Name)
            .MustAsync(async (name, token) => {
                var transaction = await repository.FindByName(name, token);
                return transaction == null;
            })
            .WithMessage("Symbol must be unique.");
    }
}


public static class ValidateExtensions {
    public static IServiceCollection AddValidator(this IServiceCollection services) => services
        .AddScoped<IValidatorAdapter, ValidatorAdapter>()
        .AddScoped<RequestValidator>();
}