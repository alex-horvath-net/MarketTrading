using Common.Data.Business.Model;

namespace Experts.Trader.FindTransactions.Read.Business;

public interface IRepositoryAdapter {
    Task<List<Transaction>> ReadTransaction(Request request, CancellationToken token);
}