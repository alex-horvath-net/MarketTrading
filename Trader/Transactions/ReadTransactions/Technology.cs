using Adapters.AppDataModel;
using Microsoft.EntityFrameworkCore;
using Technology.AppData;

namespace Trader.Transactions.ReadTransactions;
public class Technology {
    public class Repository(AppDbContext db) : Adapters.Repository.IRepository {
        public Task<List<Transaction>> Read(CancellationToken token) =>  db.Transactions.ToListAsync(token);
    }
}
 