namespace Experts.Trader.FindTransactions.Clock.Microsoft;

public class Adapter(Adapter.IClient client) : Service.IClock
{
    public DateTime GetTime() => client.Now;

    public interface IClient { DateTime Now { get; } }
}
