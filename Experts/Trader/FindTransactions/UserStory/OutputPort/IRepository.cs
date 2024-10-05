using Common.Business.Model;

namespace Experts.Trader.FindTransactions.UserStory.OutputPort;

public interface IRepository { Task<List<Transaction>> FindTransactions(InputPort.Request request, CancellationToken token); }
