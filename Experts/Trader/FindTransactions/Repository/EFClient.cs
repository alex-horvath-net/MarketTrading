using Common.Technology.EF;
using Common.Technology.EF.App;

namespace Experts.Trader.FindTransactions.Repository;

public class EFClient(EFClient.Database db) : CommonClient<EFClient.Database, EFAdapter.DataModel>(db), EFAdapter.IEFClient
{

    public Task<List<EFAdapter.DataModel>> Find(string? name, CancellationToken token) =>
        name == null ?
        Find(token) :
        Find(x => x.Name == name, token);


    public class Database : AppDB { }
}
