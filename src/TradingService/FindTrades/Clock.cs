using Microsoft.Extensions.DependencyInjection;

namespace TradingService.FindTrades;
internal class ClockAdapter() : IClockAdapter {
    public DateTime GetTime() => DateTime.UtcNow;
}

internal static class ClockExtensions {
    public static IServiceCollection AddClock(this IServiceCollection services) => services
        .AddScoped<IClockAdapter, ClockAdapter>();
}