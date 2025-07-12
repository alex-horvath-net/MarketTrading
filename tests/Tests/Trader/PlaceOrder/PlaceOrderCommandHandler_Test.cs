using TradingService.Domain;
using TradingService.Domain.Orders.CommandHendlers;
using TradingService.Domain.Orders.Commands;

namespace Tests.Trader.PlaceOrder;
public class PlaceOrderCommandHandler_Test : Order_Test_Language<PlaceOrderCommand> {
    protected override CommandHandler<PlaceOrderCommand, Guid> CommandHandler =>
        new PlaceOrderCommandHandler(base.testEventStore, base.testTime);

    [Fact]
    public void Valid_PlaceOrderCommand_Implies_OrderPlacedEvent() {
        Given();
        When(
           Valid_PlaceOrderCommand());
        Then(
            OrderPlacedEnent());
    }

    [Fact]
    public void InValid_PlaceOrderCommand_Implies_OrderPlacementFailedEvent() {
        Given();
        When(
           InValid_PlaceOrderCommand());
        Then(
            OrderPlacementFailedEvent());
    }
}
 