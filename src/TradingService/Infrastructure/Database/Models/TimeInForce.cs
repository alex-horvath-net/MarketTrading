namespace TradingService.Infrastructure.Database.Models;

public enum TimeInForce {
    Day = 0,
    GTC = 1, // Good Till Canceled
    IOC = 2  // Immediate Or Cancel
}
