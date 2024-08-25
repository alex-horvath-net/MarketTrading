using Infrastructure.Data.App;
using Infrastructure.Data.App.Model;

namespace Trader.Transactions.ReadTransactions;
public class Plugins {
    public class Repository(AppDbContext db) : Adapters.Repository.IRepository {
        public List<Transaction> Read(CancellationToken token) => db.Transactions.ToList();
    }
}
