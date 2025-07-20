namespace TradingService.Infrastructure.Database.Models;
public class Trade {
    public Guid Id { get; set; }
    public string TraderId { get; set; }
    public string Instrument { get; set; }
    public TradeSide Side { get; set; }
    public decimal Quantity { get; set; }
    public decimal? Price { get; set; } // Nullable for market orders
    public OrderType OrderType { get; set; }
    public TimeInForce TimeInForce { get; set; }
    public DateTime SubmittedAt { get; set; }
    public string StrategyCode { get; set; }
    public string PortfolioCode { get; set; }
    public string UserComment { get; set; }
    public DateTime? ExecutionRequestedForUtc { get; set; }
    public TradeStatus Status { get; set; }
    public bool IsDeleted { get; set; }

    public TradeExecutionDetail? ExecutionDetail { get; set; } // 1:1
    public ICollection<TradeLeg> Legs { get; set; } = []; // 1:*
    public ICollection<Tag> Tags { get; set; } = []; // *:*
}
