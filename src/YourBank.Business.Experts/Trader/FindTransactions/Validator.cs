using Business.Domain;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Experts.Trader.FindTransactions;

internal class ValidatorAdapter(
    IValidator<Settings> settigsValidator,
    IValidator<FindTransactionsRequest> requestValidator) : IValidatorAdapter {
    public async Task<List<Domain.Error>> Validate(
        FindTransactionsRequest request,
        Settings settings,
        CancellationToken token) {

        var settingsIssues = await settigsValidator.ValidateAsync(settings, token);
        var errors = settingsIssues.Errors.Select(MakeItFluentValidationFree).ToList();

        if (errors.Any())
            return errors;

        var requestIssues = await requestValidator.ValidateAsync(request, token);
        errors = requestIssues.Errors.Select(MakeItFluentValidationFree).ToList();

        return errors;
    }

    private static Error MakeItFluentValidationFree(ValidationFailure error) =>
         new(error.PropertyName, error.ErrorMessage);

}

internal class SettigsValidator : FluentValidation.AbstractValidator<Settings> {
    public SettigsValidator() {
        RuleFor(settings => settings)
            .NotNull()
            .WithMessage("Settings must be provided.");
        RuleFor(settings => settings.Enabled)
            .Must(enabled => enabled)
            .WithMessage("This feature is not available yet.");
    }
}

internal class RequestValidator : FluentValidation.AbstractValidator<FindTransactionsRequest> {
    public RequestValidator() {
        RuleFor(request => request)
            .NotNull()
            .WithMessage("Request must be provided.");
        RuleFor(request => request.UserId)
            .NotNull()
            .WithMessage("UserId must be provided.");
        RuleFor(request => request.Name)
            .MinimumLength(3)
            .WithMessage("Name must be at least 3 characters long.");
    }
}

internal static class ValidateExtensions {
    public static IServiceCollection AddValidator(this IServiceCollection services) => services
        .AddScoped<IValidatorAdapter, ValidatorAdapter>()
        .AddScoped<IValidator<Settings>, SettigsValidator>()
        .AddScoped<IValidator<FindTransactionsRequest>, RequestValidator>();
}