using Common.Data.Adapters;
using Common.Data.Technology;
using Experts.Trader.FindTransactions.Read.Adapters;
using Microsoft.EntityFrameworkCore;

namespace Experts.Trader.FindTransactions.Read.Technology;

public class RepositoryClient(AppDB db) : IRepositoryClient {

    public async Task<List<TransactionDM>> ReadTransaction(CancellationToken token) => await db
        .Transactions.ToListAsync(token);

    public async Task<List<TransactionDM>> ReadTransaction(string name, CancellationToken token) => await db
        .Transactions.Where(x => x.Name.Contains(name)).ToListAsync(token);
}
