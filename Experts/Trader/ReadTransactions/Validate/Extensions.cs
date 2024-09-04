using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.ReadTransactions.Validate;

public static class Extensions
{
    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services
            .AddScoped<Adapter>()
                .AddScoped<IValidator, Validator>()
                    .AddScoped<IValidator<Request>, Validator.RequestValidator>();

        return services;
    }
}

