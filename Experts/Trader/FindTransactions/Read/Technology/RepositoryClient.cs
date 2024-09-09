using Common.Data.Adapters;
using Common.Data.Technology;
using Experts.Trader.FindTransactions.Read.Adapters;

namespace Experts.Trader.FindTransactions.Read.Technology;

public class RepositoryClient(AppDB db) : DataClient<TransactionDM>(db), IRepositoryClient {
    public Task<List<TransactionDM>> Find(string? name, CancellationToken token) => name == null ?
            Find(token) :
            Find(x => x.Name == name, token);
}
