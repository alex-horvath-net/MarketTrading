using Microsoft.Extensions.DependencyInjection;

namespace Business.Experts.Trader.PlaceTrade.WorkSteps;
internal class ClockAdapter() : IClockAdapter {
    public DateTime GetTime() => DateTime.UtcNow;
}

internal static class ClockExtensions {
    public static IServiceCollection AddClock(this IServiceCollection services) => services
        .AddScoped<IClockAdapter, ClockAdapter>();
}