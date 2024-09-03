using Microsoft.EntityFrameworkCore;

namespace Experts.Account.Register;


public class RepositoryAdapterPlugin(RepositoryAdapterPlugin.RepositoryTechnologyPort repositoryTechnologyPort) : Feature.IRepositoryAdapterPort {
    public async Task<List<Common.Business.Transaction>> ReadTransaction(Feature.Request request, CancellationToken token) {
        var adapterData = await repositoryTechnologyPort.ReadTransaction(request.Name, token);
        var businessData = adapterData.Select(ToDomain).ToList();
        return businessData;
    }

    private Common.Business.Transaction ToDomain(Common.Adapters.AppDataModel.Transaction data) => new() {
        Id = data.Id,
        Name = data.Name
    };

    public interface RepositoryTechnologyPort {
        public Task<List<Common.Adapters.AppDataModel.Transaction>> ReadTransaction(string name, CancellationToken token);
    }
}

public class RepositoryTechnologyPlugin(Common.Technology.AppData.AppDB db) : RepositoryAdapterPlugin.RepositoryTechnologyPort {
    public Task<List<Common.Adapters.AppDataModel.Transaction>> ReadTransaction(string name, CancellationToken token) =>
        db.Transactions.Where(x => x.Name.Contains(name)).ToListAsync(token);
}
