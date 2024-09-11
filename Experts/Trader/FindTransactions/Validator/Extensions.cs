using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions.Validator;

public static class Extensions {

    public static IServiceCollection AddValidator(this IServiceCollection services) => services
        .AddScoped<Business.IValidator, FluentValidator.Adapter.Adapter>()
        .AddScoped<FluentValidator.Adapter.IClient, FluentValidator.Technology.Client>()
        .AddScoped<FluentValidation.IValidator<Request>, FluentValidator.Technology.Validator>();


}
