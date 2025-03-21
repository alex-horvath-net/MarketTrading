using Microsoft.Extensions.DependencyInjection;

namespace Business.Experts.Trader.FindTransactions;
public class Clock {
    public class Adapter(Adapter.IInfrastructure infra) : Featrure.IClock {
        public DateTime GetTime() => infra.Now;

        public interface IInfrastructure { DateTime Now { get; } }
    }

    public class Infrastructure : Adapter.IInfrastructure {
        public DateTime Now => DateTime.Now;
    }
}

public static class ClockExtensions {
    public static IServiceCollection AddClock(this IServiceCollection services) => services
        .AddScoped<Featrure.IClock, Clock.Adapter>()
        .AddScoped<Clock.Adapter.IInfrastructure, Clock.Infrastructure>();
}