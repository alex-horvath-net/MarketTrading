using Common.Technology.FluentValidator;
using Common.Validation.Business;
using Common.Validation.FluentValidator.Adapters;
using Common.Validation.FluentValidator.Technology;
using Experts.Trader.EditTransaction.Validate.Technology;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.EditTransaction.Validate;

public static class Extensions {
    public static IServiceCollection AddValidation(this IServiceCollection services) => services
        .AddScoped<IValidator<Request>, Validate<Request>>()
        .AddScoped<ICommonClient<Request>, CommonClient<Request>>()
        .AddScoped<IValidator<Request>, Validator>();
}
