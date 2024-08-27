using DataModel = Common.Adapters.AppDataModel;
using Common.Technology.AppData;
using Microsoft.EntityFrameworkCore;
using Experts.Trader.ReadTransactions.Adapters.TechnologyPorts;

namespace Experts.Trader.ReadTransactions.Technology;
public class RepositoryTechnologyPlugin(AppDbContext db) : IRepositoryTechnologyPort {
    public Task<List<DataModel.Transaction>> ReadTransaction(CancellationToken token) => db.Transactions.ToListAsync(token);
}
