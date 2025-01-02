using Experts.Trader.FindTransactions.Feature;
using Experts.Trader.FindTransactions.Feature.OutputPorts;

namespace Experts.Trader.FindTransactions;

public class Flag(Flag.IClient client) : IFlag {
    public bool IsPublic(Request request, CancellationToken token) {
        var isPublic = client.IsEnabled();
        token.ThrowIfCancellationRequested();
        return isPublic;
    }
    public interface IClient {
        bool IsEnabled();
    }

    public class Client : IClient {
        public bool IsEnabled() => false;
    }
}
