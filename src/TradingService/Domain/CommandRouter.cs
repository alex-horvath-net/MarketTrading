using TradingService.Domain.Orders.CommandHendlers;
using TradingService.Domain.Orders.Commands;

namespace TradingService.Domain;

// It orchestrates the event persistence process for all CommandHandlers.
public class CommandRouter<TAggregateId>(
    IEventStore<TAggregateId> eventStore,
    IServiceProvider serviceProvider
) {
    public void HandleCommand(object command) {
        var commandType = command.GetType();
        var aggregateIdType = typeof(TAggregateId);

        var commandHandlerType = typeof(CommandHandler<,>)
            .MakeGenericType(commandType, aggregateIdType);

        var commandHandlerInstance =
            serviceProvider.GetService(commandHandlerType);
        if (commandHandlerInstance == null)
            throw new InvalidOperationException(
              $"No handler registered for {commandType.Name}");

        var handleMethodInfo = commandHandlerType.GetMethod("Handle");
        if (handleMethodInfo == null)
            throw new InvalidOperationException(
               $"Handle method not found on {commandHandlerType.Name}");

        handleMethodInfo.Invoke(
           commandHandlerInstance,
           new[] { command });

        eventStore.SaveChanges();
    }
}


public static class Extensions {
    public static void AddPlaceOrder(this IServiceCollection services) {
        services.AddScoped<CommandRouter<Guid>>();

        services.AddTransient<CommandHandler<PlaceOrderCommand,Guid>, PlaceOrderCommandHandler>();
    }
}