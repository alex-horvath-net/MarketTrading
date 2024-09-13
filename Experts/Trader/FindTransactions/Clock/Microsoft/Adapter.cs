using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions.Clock.Microsoft;

public class Adapter(Adapter.IClient client) : Service.IClock
{
    public DateTime GetTime() => client.Now;

    public interface IClient { DateTime Now { get; } }
}

public static class AdapterExtensions {

    public static IServiceCollection AddClockAdapter(this IServiceCollection services) => services
        .AddScoped<Service.IClock, Adapter>()
        .AddClockClient();
} 