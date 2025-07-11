namespace TradingService.Domain.Orders.Events;

public record OrderPlacementFailedEvent(
  OrderPlacementFailedEvent.FailReson Reason) : EventBase {

    public enum FailReson {
        InvalidQuantity,
        InvalidPrice
    }

}
