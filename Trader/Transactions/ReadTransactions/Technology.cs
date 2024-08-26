using Adapters.AppDataModel;
using Adapters.Plugins.AppData;
using Microsoft.EntityFrameworkCore;

namespace Trader.Transactions.ReadTransactions;
public class Technology {
    public class Repository(AppDbContext db) : Adapters.Repository.IRepository {
        public Task<List<Transaction>> Read(CancellationToken token) =>  db.Transactions.ToListAsync(token);
    }
}
 