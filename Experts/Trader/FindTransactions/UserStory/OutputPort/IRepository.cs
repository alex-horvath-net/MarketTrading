using Common.Business.Model;

namespace DomainExperts.Trader.FindTransactions.UserStory.OutputPort;

public interface IRepository { Task<List<Transaction>> FindTransactions(InputPort.Request request, CancellationToken token); }
