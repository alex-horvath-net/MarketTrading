

using Common.Adapters.AppDataModel;
using Experts.Trader.ReadTransactions.Adapters.TechnologyPorts;
using Experts.Trader.ReadTransactions.Business.AdapterPorts;

namespace Experts.Trader.ReadTransactions.Adapters;
public class RepositoryAdapterPlugin(IRepositoryTechnologyPort repositoryTechnologyPort) : IRepositoryAdapterPort {
    public async Task<List<Common.Business.Transaction>> ReadTransaction(Business.Request request, CancellationToken token) {
        var adapterData = await repositoryTechnologyPort.ReadTransaction(token);
        var businessData = adapterData.Select(ToDomain).ToList();
        return businessData;
    }

    private Common.Business.Transaction ToDomain(Transaction data) => new() {
        Id = data.Id
    };
}
