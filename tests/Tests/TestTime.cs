using TradingService.Domain;

namespace Tests;

public class TestTime : ITime {
    public DateTime UtcNow => new (2020,2,2);
}
