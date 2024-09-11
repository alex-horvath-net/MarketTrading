using Experts.Trader.FindTransactions.Clock.Microsoft.Adapter;

namespace Experts.Trader.FindTransactions.Clock.Microsoft.Technology;

public class Client : IClient {
    public DateTime Now => DateTime.Now;
}
