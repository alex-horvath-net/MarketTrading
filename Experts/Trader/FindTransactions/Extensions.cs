using Experts.Trader.FindTransactions.Read;
using Experts.Trader.FindTransactions.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions;

public static class Extensions {
    public static IServiceCollection AddFindTransactions(this
        IServiceCollection services,
        ConfigurationManager configuration) {
        services.AddScoped<Service>();
        services.AddValidation();
        services.AddRead(configuration);
        return services;
    }
}
