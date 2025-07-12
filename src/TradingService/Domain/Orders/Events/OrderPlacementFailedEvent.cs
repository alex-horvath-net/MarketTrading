namespace TradingService.Domain.Orders.Events;

public record OrderPlacementFailedEvent(
  OrderPlacementFailedEvent.FailReson Reason, DateTime Timestamp) : EventBase(Timestamp) {

    public enum FailReson {
        InvalidQuantity,
        InvalidPrice
    }

}
