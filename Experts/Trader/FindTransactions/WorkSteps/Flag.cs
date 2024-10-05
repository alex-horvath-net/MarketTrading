using Experts.Trader.FindTransactions.UserStory.OutputPort;

namespace Experts.Trader.FindTransactions.WorkSteps;

public class Flag(Flag.IClient client) : IFlag {
    public bool IsPublic(UserStory.InputPort.Request request, CancellationToken token) {
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
