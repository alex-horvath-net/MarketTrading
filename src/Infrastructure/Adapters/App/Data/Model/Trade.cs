namespace Infrastructure.Adapters.App.Data.Model;
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
}

public enum TradeSide {
    Buy = 0,
    Sell = 1
}

public enum OrderType {
    Market = 0,
    Limit = 1,
    Stop = 2
}

public enum TimeInForce {
    Day = 0,
    GTC = 1, // Good Till Canceled
    IOC = 2  // Immediate Or Cancel
}

public enum TradeStatus {
    Submitted = 0,
    Executed = 1,
    Canceled = 2
}