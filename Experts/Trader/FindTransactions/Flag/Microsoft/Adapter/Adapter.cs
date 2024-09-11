using Experts.Trader.FindTransactions.Flag.Busines;

namespace Experts.Trader.FindTransactions.Flag.Microsoft.Adapter;

public class Adapter(IClient client) : IFlag {
    public bool IsPublic(Request request, CancellationToken token) => client.IsEnabled();
}
