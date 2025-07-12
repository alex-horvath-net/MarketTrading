using TradingService.Domain.Orders.CommandHendlers;
using TradingService.Domain.Orders.Commands;

namespace TradingService.Domain;

public static class DomainExtensions {
    public static void AddDomain(this IServiceCollection services) {
        services.AddScoped<CommandRouter<Guid>>();

        services.AddTransient<CommandHandler<PlaceOrderCommand,Guid>, PlaceOrderCommandHandler>();
    }
} 