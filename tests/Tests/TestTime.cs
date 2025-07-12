using TradingService.Domain;

namespace Tests;

public class TestTime : ITime {
    public DateTime Now => new (2020,2,2);
}
