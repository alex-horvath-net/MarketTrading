using Microsoft.Extensions.DependencyInjection;

namespace Business.Experts.Trader.FindTrades;
internal class ClockAdapter() : IClockAdapter {
    public DateTime GetTime() => DateTime.UtcNow;
}

internal static class ClockExtensions {
    public static IServiceCollection AddClock(this IServiceCollection services) => services
        .AddScoped<IClockAdapter, ClockAdapter>();
}