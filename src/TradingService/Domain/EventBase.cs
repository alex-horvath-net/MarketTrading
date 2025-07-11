namespace TradingService.Domain;
public abstract record EventBase {
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}
