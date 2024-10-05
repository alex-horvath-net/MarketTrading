namespace Experts.Trader.FindTransactions.UserStory.OutputPort;

public interface IFlag { bool IsPublic(InputPort.Request request, CancellationToken token); }
