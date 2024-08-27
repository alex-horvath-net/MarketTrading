using Microsoft.EntityFrameworkCore;
using Experts.Trader.ReadTransactions.Adapters.TechnologyPorts;

namespace Experts.Trader.ReadTransactions.Technology;
public class RepositoryTechnologyPlugin(Common.Technology.AppData.AppDbContext db) : IRepositoryTechnologyPort {
    public Task<List<Common.Adapters.AppDataModel.Transaction>> ReadTransaction(CancellationToken token) => db.Transactions.ToListAsync(token);
}
