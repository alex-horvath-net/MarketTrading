using TradingService.Domain;
using TradingService.Domain.Orders.CommandHendlers;
using TradingService.Domain.Orders.Commands;
using TradingService.Domain.Orders.Events;
using TradingService.Domain.Orders.ValueTypes;

namespace Tests.Trader.PlaceOrder;
public class PlaceOrderCommandHandler_Test : TestCommandHandler<PlaceOrderCommand> {
    protected override CommandHandler<PlaceOrderCommand, Guid> CommandHandler =>
        new PlaceOrderCommandHandler(base.testEventStore, base.testTime);

    [Fact]
    public void WhenUsingValidLabel_ShouldAddLabel() {
        Given();
        When(
           new PlaceOrderCommand(_defaultAggregateId, "AAPL", 10, new Price(150.00m)));
        Then(
            new OrderPlacedEvent(_defaultAggregateId, "AAPL", 10, new Price(150.00m), testTime.UtcNow));
    }


}
