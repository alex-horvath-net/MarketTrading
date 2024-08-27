using DataModel = Common.Adapters.AppDataModel;

namespace Experts.Trader.ReadTransactions.Adapters.TechnologyPorts;
public interface IRepositoryTechnologyPort {
    public Task<List<DataModel.Transaction>> ReadTransaction(CancellationToken token);
}
