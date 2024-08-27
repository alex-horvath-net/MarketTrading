namespace Experts.Trader.ReadTransactions.Adapters.TechnologyPorts;
public interface IRepositoryTechnologyPort {
    public Task<List<Common.Adapters.AppDataModel.Transaction>> ReadTransaction(CancellationToken token);
}
