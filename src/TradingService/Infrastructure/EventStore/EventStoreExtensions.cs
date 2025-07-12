using TradingService.Infrastructure.EventStore;

namespace TradingService.Domain;

public static class EventStoreExtensions {
    public static void AddEventStore(this IServiceCollection services) {
        services.AddScoped<IEventStore<Guid>, EventStore>();
    }
}