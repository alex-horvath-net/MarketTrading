using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions.Flag.Microsoft;

public class Client : Adapter.IClient
{
    public bool IsEnabled() => false;
}

public static class ClientExtensions {

    public static IServiceCollection AddFlagClient(this IServiceCollection services) => services
        .AddScoped<Adapter.IClient, Client>();
} 