using Experts.Trader.FindTransactions.Clock.Microsoft;
using Experts.Trader.FindTransactions.Flag.Microsoft;
using Experts.Trader.FindTransactions.Repository.EntityFramework;
using Experts.Trader.FindTransactions.Validator.FluentValidator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions;

public static class Extensions {

    public static IServiceCollection AddFindTransactions(this IServiceCollection services, ConfigurationManager configuration) => services
        .AddScoped<Service>()
        .AddFlag()
        .AddClock()
        .AddValidator()
        .AddRepository(configuration);
}

