namespace Business.Experts.Trader.FindTransactions.Clock;

public class DefaultClock : IClock {
    public DateTime Now => DateTime.Now;
}