using Experts.Trader.ReadTransactions.Business;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.ReadTransactions;


public class ValidatorAdapterPlugin(ValidatorAdapterPlugin.IValidatorTechnologyPort validatorTechnologyPort) : Feature.IValidatorAdapterPort {
    public async Task<List<string>> Validate(Feature.Request request, CancellationToken token) {
        var adapterData = await validatorTechnologyPort.Validate(request, token);
        var businessData = adapterData;
        return businessData;
    }

    public interface IValidatorTechnologyPort {
        public Task<List<string>> Validate(Feature.Request request, CancellationToken token);
    }
}


public class ValidatorTechnologyPlugin(IValidator<Feature.Request> validator) : ValidatorAdapterPlugin.IValidatorTechnologyPort {
    public async Task<List<string>> Validate(Feature.Request request, CancellationToken token) {
        var validationResult = await validator.ValidateAsync(request, token);
        return validationResult.Errors.Select(e => e.ErrorMessage).ToList();
    }

    public class RequestValidator : AbstractValidator<Feature.Request> {
        public RequestValidator() {

            RuleFor(request => request)
                .NotNull().WithMessage(Mesages.RequestNull);

            RuleFor(request => request.Name)
                .MinimumLength(3).When(request => !string.IsNullOrEmpty(request.Name)).WithMessage(Mesages.NameShort);
        }
    }

    public static class Mesages {
        public static string RequestNull => "Request cannot be null.";
        public static string NameShort => "Name must be at least 3 characters long if provided.";
    }
}

public static class ValidateExtensions {
    public static IServiceCollection AddValidation(this IServiceCollection services) {
        services
            .AddScoped<Feature.IValidatorAdapterPort, ValidatorAdapterPlugin>()
                .AddScoped<ValidatorAdapterPlugin.IValidatorTechnologyPort, ValidatorTechnologyPlugin>()
                    .AddScoped<IValidator<Feature.Request>, ValidatorTechnologyPlugin.RequestValidator>();

        return services;
    }
}

