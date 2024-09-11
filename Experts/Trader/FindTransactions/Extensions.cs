using Experts.Trader.FindTransactions.Repository;
using Experts.Trader.FindTransactions.Validator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions;

public static class Extensions {

    public static IServiceCollection AddFindTransactions(this IServiceCollection services, ConfigurationManager configuration) => services
        .AddScoped<Service>()
        .AddValidator()
        .AddRepository(configuration);

}
