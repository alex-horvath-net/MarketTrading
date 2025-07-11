using TradingService.Domain.Orders.Events;
using TradingService.Domain.Orders.ValueTypes;

namespace TradingService.Domain.Orders.AggregetRoot;

public class Order : AggregateRoot<Guid> {
    public string Symbol { get; private set; }
    public int Quantity { get; private set; }
    public Price Price { get; private set; }
    public bool IsPlaced { get; private set; }

    public void Apply(OrderPlacedEvent businesEvent) {
        Symbol = businesEvent.Symbol;
        Quantity = businesEvent.Quantity;
        Price = businesEvent.Price;
        IsPlaced = true;
    }
}