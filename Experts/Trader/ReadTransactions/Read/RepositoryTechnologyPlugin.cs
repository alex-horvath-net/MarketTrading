using Microsoft.EntityFrameworkCore;

namespace Experts.Trader.ReadTransactions.Read;


public class RepositoryTechnologyPlugin(Common.Technology.AppData.AppDbContext db) : RepositoryAdapterPlugin.RepositoryTechnologyPort {
    public Task<List<Common.Adapters.AppDataModel.Transaction>> ReadTransaction(CancellationToken token) => db.Transactions.ToListAsync(token);
}
