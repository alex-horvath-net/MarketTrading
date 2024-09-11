using Common.Adapters.App.Data.Model;
using Common.Technology.EF.App;
using Microsoft.EntityFrameworkCore;

namespace Experts.Trader.FindTransactions.Repository.EntityFramework;

public class Client(AppDB db) : Adapter.IClient {
    public Task<List<TransactionDM>> Find(string? name, CancellationToken token) =>
        name == null ?
        db.Transactions.ToListAsync(token) :
        db.Transactions.Where(x => x.Name == name).ToListAsync(token);
}
