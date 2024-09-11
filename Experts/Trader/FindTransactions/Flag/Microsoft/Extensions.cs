using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions.Flag.Microsoft;

public static class Extensions {

    public static IServiceCollection AddFlag(this IServiceCollection services) => services
        .AddScoped<Service.IFlag, Adapter>()
        .AddScoped<Adapter.IClient, Client>();
}
