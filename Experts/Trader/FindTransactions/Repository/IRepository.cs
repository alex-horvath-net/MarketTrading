using Common.Business.Model;

namespace Experts.Trader.FindTransactions.Repository;

public interface IRepository { Task<List<Transaction>> FindTransactions(Request request, CancellationToken token); }
