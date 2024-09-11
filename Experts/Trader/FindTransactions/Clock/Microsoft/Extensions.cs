using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions.Clock.Microsoft;

public static class Extensions {

    public static IServiceCollection AddClock(this IServiceCollection services) => services
        .AddScoped<Service.IClock, Adapter>()
        .AddScoped<Adapter.IClient, Client>();
}
 