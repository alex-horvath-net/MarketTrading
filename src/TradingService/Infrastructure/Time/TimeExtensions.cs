using TradingService.Domain;

namespace TradingService.Infrastructure.Time;

public static class TimeExtensions {
    public static IServiceCollection AdddLondonTime(this IServiceCollection services) {
        services.AddScoped<ITime, LondonTime>();
        return services;
    }
}