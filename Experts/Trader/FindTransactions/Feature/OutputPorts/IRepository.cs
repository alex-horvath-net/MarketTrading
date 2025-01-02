using Domain;

namespace DomainExperts.Trader.FindTransactions.Feature.OutputPorts;

public interface IRepository { Task<List<Trade>> FindTransactions(Request request, CancellationToken token); }

