using Experts.Trader.FindTransactions.Clock.Business;

namespace Experts.Trader.FindTransactions.Clock.Microsoft.Adapter;

public class Adapter(IClient client) : IClock {
    public DateTime GetTime() => client.Now;
}
