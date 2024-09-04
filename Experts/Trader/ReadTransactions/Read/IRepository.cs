using Common.Adapters.AppDataModel;

namespace Experts.Trader.ReadTransactions.Read;

public interface IRepository
{
    public Task<List<TransactionDM>> ReadTransaction(CancellationToken token);
    public Task<List<TransactionDM>> ReadTransaction(string name, CancellationToken token);
}
