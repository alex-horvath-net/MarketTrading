using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions.Clock.Microsoft;

public class Client : Adapter.IClient
{
    public DateTime Now => DateTime.Now;
}

public static class ClientExtensions {

    public static IServiceCollection AddClockClient(this IServiceCollection services) => services
        .AddScoped<Adapter.IClient, Client>();
}