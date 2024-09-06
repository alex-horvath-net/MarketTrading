using Common.Data.Adapters;

namespace Experts.Trader.FindTransactions.Read.Adapters;

public interface IRepositoryClient
{
    public Task<List<TransactionDM>> ReadTransaction(CancellationToken token);
    public Task<List<TransactionDM>> ReadTransaction(string name, CancellationToken token);
}
