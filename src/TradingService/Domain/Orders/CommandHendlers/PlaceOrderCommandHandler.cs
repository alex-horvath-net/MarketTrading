using TradingService.Domain.Orders.AggregetRoot;
using TradingService.Domain.Orders.Commands;
using TradingService.Domain.Orders.Events;

namespace TradingService.Domain.Orders.CommandHendlers;

// It is the main busines logic controller.
// Note, I don't persist here the events to the EventStore,
// So I can handele mutiple commands before actually persist.
public class PlaceOrderCommandHandler(IEventStore<Guid> eventStore, ITime time) : CommandHandler<PlaceOrderCommand, Guid>(eventStore) {

    public override void Handle(PlaceOrderCommand command) {
        var eventStream = base.GetEventStream<Order>(command.OrderId);
        var order = eventStream.GetAggregateRoot();
        if (command.Price.IsValid()) {
            var orderPlacedEvent = new OrderPlacedEvent(
                command.OrderId,
                command.Symbol,
                command.Quantity,
                command.Price,
       time.UtcNow);

            eventStream.Append(orderPlacedEvent);
        } else {
            var orderPlacementFailedEvent =
                new OrderPlacementFailedEvent(
                     OrderPlacementFailedEvent.FailReson.InvalidPrice,
                     time.UtcNow);
            eventStream.Append(orderPlacementFailedEvent);
        }
    }
}