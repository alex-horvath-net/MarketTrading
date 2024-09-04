using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.ReadTransactions.Business;

public static class Extensions
{
    public static IServiceCollection AddReadTransactions(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddScoped<Feature>();
        services.AddValidation();
        services.AddRead(configuration);
        return services;
    }
}
