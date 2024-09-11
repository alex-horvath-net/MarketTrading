using Common.Adapters.App.Data;
using Common.Adapters.App.Data.Model;
using Common.Business.Model;

namespace Experts.Trader.FindTransactions.Repository;

public class EFAdapter(EFAdapter.IEFClient client) : IRepository
{

    public async Task<List<Transaction>> FindTransactions(Request request, CancellationToken token)
    {
        var dataModelList = await client.Find(request.Name, token);
        var businessModelList = dataModelList.Select(ToBusinessModel).ToList();
        return businessModelList;
    }

    private static Transaction ToBusinessModel(DataModel dataModel) => new()
    {
        Id = dataModel.Id,
        Name = dataModel.Name
    };

    public interface IEFClient : ICommonEFClient<DataModel>
    {
        public Task<List<DataModel>> Find(string? name, CancellationToken token);
    }

    public class DataModel : TransactionDM;
}
