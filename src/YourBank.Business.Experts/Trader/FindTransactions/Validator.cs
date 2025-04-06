using Business.Domain;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Experts.Trader.FindTransactions;

internal class ValidatorAdapter(
    SettigsValidator settigsValidator,
    RequestValidator requestValidator) : IValidatorAdapter {
    public async Task<List<Domain.Error>> Validate(
        Request request,
        Settings settings,
        CancellationToken token) {
        var erors = new List<Domain.Error>();

        var settingsIssues = await settigsValidator.ValidateAsync(
            settings,
            token);
        foreach (var error in settingsIssues.Errors)
            erors.Add(MakeInfrastructureFree(error));

        if (erors.Any())
            return erors;

        var requestIssues = await requestValidator.ValidateAsync(
            request,
            token);
        foreach (var error in requestIssues.Errors)
            erors.Add(
                new Domain.Error(
                    error.PropertyName,
                    error.ErrorMessage));

        return erors;
    }

    private static Error MakeInfrastructureFree(ValidationFailure error) =>
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

internal class RequestValidator : FluentValidation.AbstractValidator<Request> {
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
        .AddScoped<IValidator<Request>, RequestValidator>();
}