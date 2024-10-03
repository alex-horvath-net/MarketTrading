using Common;
using Common.Adapters.App.Data.Model;
using Common.Business.Model;
using Common.Technology.EF.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions.WorkSteps;

public class Repository(Repository.IClient client) : BusinessNeed.IRepository {
    public async Task<List<Transaction>> FindTransactions(Request request, CancellationToken token) {
        var dataModel = await client.Find(request.Name, token);
        var businessModel = dataModel.Select(ToBusinessModel).ToList();

        token.ThrowIfCancellationRequested();
        return businessModel;
    }

    private static List<Transaction> ToBusinessModelList(List<TransactionDM> dataModelList) => dataModelList.Select(ToBusinessModel).ToList();
    private static Transaction ToBusinessModel(TransactionDM dataModel) => new() {
        Id = dataModel.Id,
        Name = dataModel.Name
    };


    public interface IClient {
        public Task<List<TransactionDM>> Find(string? name, CancellationToken token);
    }
    public class Client(AppDB db) : IClient {
        public async Task<List<TransactionDM>> Find(string? name, CancellationToken token) {
            token.ThrowIfCancellationRequested();

            var transactions = name == null ?
               await db.Transactions.AsNoTracking().ToListAsync(token) :
               await db.Transactions.AsNoTracking().Where(x => x.Name.Contains(name)).ToListAsync(token);

            token.ThrowIfCancellationRequested();

            return transactions;
        }
    }

}

public static class RepositoryExtensions {
    public static IServiceCollection AddRepository(this IServiceCollection services) {

        return services
            .AddScoped<BusinessNeed.IRepository, Repository>()
            .AddScoped<Repository.IClient, Repository.Client>();

    }
}
