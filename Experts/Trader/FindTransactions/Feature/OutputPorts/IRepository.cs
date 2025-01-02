using Common.Business.Model;

namespace DomainExperts.Trader.FindTransactions.Feature.OutputPorts;

public interface IRepository { Task<List<Transaction>> FindTransactions(Request request, CancellationToken token); }

