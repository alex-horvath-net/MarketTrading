using Common.Adapters.App.Data.Model;
using Common.Business.Model;
using Common.Technology.EF.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions.Repository.EntityFramework;

public class Adapter(Adapter.IClient client) : Service.IRepository
{
    public async Task<List<Transaction>> FindTransactions(Service.Request request, CancellationToken token)
    {
        var dataModelList = await client.Find(request.Name, token);
        var businessModelList = dataModelList.Select(ToBusinessModel).ToList();
        token.ThrowIfCancellationRequested();
        return businessModelList;
    }

    private static Transaction ToBusinessModel(TransactionDM dataModel) => new()
    {
        Id = dataModel.Id,
        Name = dataModel.Name
    };


    public interface IClient
    {
        public Task<List<TransactionDM>> Find(string? name, CancellationToken token);
    }

    public class Client(AppDB db) : Adapter.IClient {

        public Task<List<TransactionDM>> Find(string? name, CancellationToken token) {
            token.ThrowIfCancellationRequested();

            var transactions = name == null ?
                db.Transactions.ToListAsync(token) :
                db.Transactions.Where(x => x.Name.Contains(name)).ToListAsync(token);

            token.ThrowIfCancellationRequested();
            return transactions;
        }
    }
}

public static class AdapterExtensions {
    public static IServiceCollection AddRepositoryAdapter(this IServiceCollection services, ConfigurationManager configuration) => services
        .AddScoped<Service.IRepository, Adapter>()
        .AddRepositoryClient(configuration);
}
