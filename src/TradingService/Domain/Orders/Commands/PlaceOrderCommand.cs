using TradingService.Domain.Orders.ValueTypes;

namespace TradingService.Domain.Orders.Commands;

// It is an immutable intent to change the system state.
public record PlaceOrderCommand(
    Guid OrderId,
    string Symbol,
    int Quantity,
    Price Price);
