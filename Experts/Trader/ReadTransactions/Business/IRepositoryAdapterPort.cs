using Common.Business.Model;

namespace Experts.Trader.ReadTransactions.Business;

public interface IRepositoryAdapterPort {
    public Task<List<TransactionBM>> ReadTransaction(Request request, CancellationToken token);
}
