using Common.Data.Adapters;

namespace Experts.Trader.FindTransactions.Read.Adapters;

public interface IRepositoryClient : IDataClient<TransactionDM> {
    public Task<List<TransactionDM>> Find(string? name, CancellationToken token);
}
