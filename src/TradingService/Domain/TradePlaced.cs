namespace TradingService.Domain;

public class TradePlaced : IDomainEvent {
    public Guid TradeId { get; }
    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public TradePlaced(Guid tradeId) {
        TradeId = tradeId;
    }
}
