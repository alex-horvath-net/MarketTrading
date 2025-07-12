using TradingService.Domain.Orders.Commands;
using TradingService.Domain.Orders.Events;
using TradingService.Domain.Orders.ValueTypes;
using static TradingService.Domain.Orders.Events.OrderPlacementFailedEvent;

namespace Tests.Trader.PlaceOrder;

public abstract class Order_Test_Language<TCommand> : Test_Language<TCommand> {
    protected Guid Order_Id => _defaultAggregateId;

    
    protected PlaceOrderCommand Valid_PlaceOrderCommand() => new (
            _defaultAggregateId,
            "AAPL", 
            10,
            new Price(150.00m));

    protected OrderPlacedEvent OrderPlacedEnent() => new (
            _defaultAggregateId,
            "AAPL",
            10,
            new Price(150.00m),
            testTime.Now);


    protected PlaceOrderCommand InValid_PlaceOrderCommand() =>
        Valid_PlaceOrderCommand() with { Price = new Price(0.00m) };

    protected OrderPlacementFailedEvent OrderPlacementFailedEvent() => new(
        FailReson.InvalidPrice, testTime.Now);

}
