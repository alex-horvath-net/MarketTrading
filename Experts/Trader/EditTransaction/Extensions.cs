using Experts.Trader.EditTransaction.Repository.EntityFramework;
using Experts.Trader.EditTransaction.Validator.FluentValidator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.EditTransaction;

public static class Extensions {
    public static IServiceCollection AddEditTransaction(this IServiceCollection services, ConfigurationManager configuration) {
        services.AddScoped<Service>();
        services.AddValidator();
        services.AddRepository(configuration);
        return services;
    }
}
