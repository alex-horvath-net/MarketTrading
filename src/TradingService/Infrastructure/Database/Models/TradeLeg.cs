namespace TradingService.Infrastructure.Database.Models;

// Represents the underlying breakdown of a trade, e.g., for multi-leg strategies.
public class TradeLeg {
    public Guid Id { get; set; }
    public Guid TradeId { get; set; }

    public string Instrument { get; set; } = null!;
    public TradeSide Side { get; set; }
    public decimal Quantity { get; set; }
    public decimal? Price { get; set; }

    public Trade Trade { get; set; } = null!; // *:1
    public TradeLegDetail? Detail { get; set; }
}

public class TradeLegDetail {
    public Guid Id { get; set; } // Same as TradeLeg.Id
    public string ClearingBroker { get; set; } = string.Empty;
    public string ExecutionVenue { get; set; } = string.Empty;
    public string SettlementCurrency { get; set; } = string.Empty;

    // Navigation
    public TradeLeg TradeLeg { get; set; } = null!; // 1:1
}