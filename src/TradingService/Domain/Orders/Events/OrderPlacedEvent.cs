using TradingService.Domain.Orders.ValueTypes;

namespace TradingService.Domain.Orders.Events;
// It is an immutable announcement that the system state has been changed.
public record OrderPlacedEvent(
    Guid OrderId,
    string Symbol,
    int Quantity,
    Price Price,
    DateTime Timestamp) : EventBase(Timestamp);
