using Common.Data.Adapters;
using Common.Data.Business.Model;
using Experts.Trader.FindTransactions.Read.Business;

namespace Experts.Trader.FindTransactions.Read.Adapters;

public class RepositoryAdapter(IRepositoryClient repositoryClient) : IRepositoryAdapter {
    public async Task<List<Transaction>> ReadTransaction(Request request, CancellationToken token) {
        var clientModel = await repositoryClient.Find(request.Name, token);
        var businessModel = clientModel.Select(ToBusinessModel).ToList();
        return businessModel;
    }

    private static Transaction ToBusinessModel(TransactionDM technologyModel) => new() {
        Id = technologyModel.Id,
        Name = technologyModel.Name
    };
}
