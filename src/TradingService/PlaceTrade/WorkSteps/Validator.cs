using Domain;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using TradingService.PlaceTrade;

namespace TradingService.PlaceTrade.WorkSteps;

internal class ValidatorAdapter(
    IValidator<Settings> settigsValidator,
    IValidator<PlaceTradeRequest> requestValidator) : IValidatorAdapter {
    public async Task<List<Error>> Validate(
        PlaceTradeRequest request,
        Settings settings,
        CancellationToken token) {

        var settingsIssues = await settigsValidator.ValidateAsync(settings, token);
        var errors = settingsIssues.Errors.Select(MakeItFluentValidationFree).ToList();

        if (errors.Any())
            return errors;

        var requestIssues = await requestValidator.ValidateAsync(request, token);
        errors = requestIssues.Errors.Select(MakeItFluentValidationFree).ToList();

        if (errors.Any())
            return errors;

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

internal class RequestValidator : AbstractValidator<PlaceTradeRequest> {
    public RequestValidator(IRepository repository) {
        RuleFor(request => request)
            .NotNull()
            .WithMessage("Request must be provided.");

        RuleFor(request => request.TraderId)
            .NotNull()
            .WithMessage("TraderId must be provided.");

        RuleFor(request => request.Instrument)
            .NotEmpty()
            .WithMessage("Instrument must be provided.")
            .MinimumLength( 3)
            .WithMessage("Instrument must be at least 3 characters long.");

        //RuleFor(request => request.Instrument)
        //    .MustAsync(async (instrument, token) => null != await repository.FindByName(instrument, token))
        //    .WithMessage("A trade with the same details already exists.");
    }
}


internal static class ValidateExtensions {
    public static IServiceCollection AddValidator(this IServiceCollection services) => services
        .AddScoped<IValidatorAdapter, ValidatorAdapter>()
        .AddScoped<IValidator<Settings>, SettigsValidator>()
        .AddScoped<IValidator<PlaceTradeRequest>, RequestValidator>();
}