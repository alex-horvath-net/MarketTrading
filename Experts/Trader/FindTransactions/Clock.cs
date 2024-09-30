using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions;

public class Clock(Clock.IClient client) : Service.IClock
{
    public DateTime GetTime() => client.Now;

    public interface IClient { DateTime Now { get; } }

    public class Client : IClient
    {
        public DateTime Now => DateTime.Now;
    }
}

public static class ClockExtensions
{
    public static IServiceCollection AddClock(this IServiceCollection services) => services
        .AddScoped<Service.IClock, Clock>()
        .AddScoped<Clock.IClient, Clock.Client>();
}