using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions.Validator.FluentValidator;

public static class Extensions
{

    public static IServiceCollection AddValidator(this IServiceCollection services) => services
        .AddScoped<Service.IValidator, Adapter>()
        .AddScoped<Adapter.IClient, Client>()
        .AddScoped<FluentValidation.IValidator<Service.Request>, Technology>();
}

