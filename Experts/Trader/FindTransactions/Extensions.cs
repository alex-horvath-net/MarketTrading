using Experts.Trader.FindTransactions.Read;
using Experts.Trader.FindTransactions.Validate;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions;

public static class Extensions {
    public static IServiceCollection AddFindTransactions(this IServiceCollection services, ConfigurationManager configuration) => services
        .AddScoped<WorkFlow>()
        .AddValidation()
        .AddRead(configuration);
}
