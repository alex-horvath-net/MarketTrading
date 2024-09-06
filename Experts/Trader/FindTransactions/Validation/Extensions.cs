using Common.Valdation.Adapters.Fluentvalidation;
using Common.Valdation.Business;
using Common.Valdation.Technology.FluentValidation;
using Experts.Trader.FindTransactions.Validation.Technology;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions.Validation;

public static class Extensions
{
    public static IServiceCollection AddValidation(this IServiceCollection services) => services
        .AddScoped<IValidatorAdapter<Request>, ValidatorAdapter<Request>>()
        .AddScoped<IValidatorClient<Request>, ValidatorClient<Request>>()
        .AddScoped<IValidator<Request>, Validator>();
}
