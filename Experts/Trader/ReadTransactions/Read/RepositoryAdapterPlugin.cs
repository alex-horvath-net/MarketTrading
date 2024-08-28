namespace Experts.Trader.ReadTransactions.Read;


public class RepositoryAdapterPlugin(RepositoryAdapterPlugin.RepositoryTechnologyPort repositoryTechnologyPort) : Feature.IRepositoryAdapterPort {
    public async Task<List<Common.Business.Transaction>> ReadTransaction(Request request, CancellationToken token) {
        var adapterData = await repositoryTechnologyPort.ReadTransaction(token);
        var businessData = adapterData.Select(ToDomain).ToList();
        return businessData;
    }

    private Common.Business.Transaction ToDomain(Common.Adapters.AppDataModel.Transaction data) => new() {
        Id = data.Id
    };

    public interface RepositoryTechnologyPort {
        public Task<List<Common.Adapters.AppDataModel.Transaction>> ReadTransaction(CancellationToken token);
    }
}


