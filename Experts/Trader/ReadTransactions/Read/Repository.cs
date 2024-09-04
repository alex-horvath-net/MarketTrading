using Common.Adapters.AppDataModel;
using Microsoft.EntityFrameworkCore;

namespace Experts.Trader.ReadTransactions.Read;

public class Repository(Common.Technology.AppData.AppDB db) : IRepository
{

    public async Task<List<TransactionDM>> ReadTransaction(CancellationToken token) =>
        await db.Transactions.ToListAsync(token);

    public async Task<List<TransactionDM>> ReadTransaction(string name, CancellationToken token) =>
        await db.Transactions.Where(x => x.Name.Contains(name)).ToListAsync(token);
}
