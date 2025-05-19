using Domain;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;

namespace TradingService.FindTrades;

internal class ValidatorAdapter(
    IValidator<Settings> settigsValidator,
    IValidator<FindTradesRequest> requestValidator) : IValidatorAdapter {
    public async Task<List<Error>> Validate(
        FindTradesRequest request,
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

internal class SettigsValidator : AbstractValidator<Settings> {
    public SettigsValidator() {
        RuleFor(settings => settings)
            .NotNull()
            .WithMessage("Settings must be provided.");
        RuleFor(settings => settings.Enabled)
            .Must(enabled => enabled)
            .WithMessage("This feature is not available yet.");
    }
}

internal class RequestValidator : AbstractValidator<FindTradesRequest> {
    public RequestValidator() {
        RuleFor(request => request)
            .NotNull()
            .WithMessage("Request must be provided.");
       
        RuleFor(request => request.Id)
            .NotNull()
            .WithMessage("UserId must be provided.");
       
        RuleFor(request => request.Instrument)
            .Must(name => string.IsNullOrEmpty(name) || name.Length >= 3)
            .WithMessage("Instrument must be at least 3 characters long or empty.");
    }
}

internal static class ValidateExtensions {
    public static IServiceCollection AddValidator(this IServiceCollection services) => services
        .AddScoped<IValidatorAdapter, ValidatorAdapter>()
        .AddScoped<IValidator<Settings>, SettigsValidator>()
        .AddScoped<IValidator<FindTradesRequest>, RequestValidator>();
}