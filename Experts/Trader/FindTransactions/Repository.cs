using Common.Adapters.App.Data.Model;
using Common.Business.Model;
using Common.Technology.EF.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions;

public class Repository(Repository.IClient client) : Service.IRepository {
    public async Task<List<Transaction>> FindTransactions(Request request, CancellationToken token) {
        var dataModelList = await client.Find(request.Name, token);
        var businessModelList = dataModelList.Select(ToBusinessModel).ToList();
        token.ThrowIfCancellationRequested();
        return businessModelList;
    }

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

            List<TransactionDM> transactions = default;
            try {
                transactions = name == null ?
                   await db.Transactions.ToListAsync(token) :
                   await db.Transactions.Where(x => x.Name.Contains(name)).ToListAsync(token);
            } catch (Exception e) {
                throw;
            }
            token.ThrowIfCancellationRequested();
            return transactions;
        }
    }

}

public static class RepositoryExtensions {
    public static IServiceCollection AddRepository(this IServiceCollection services, ConfigurationManager configuration) {

        var connectionString = configuration.GetConnectionString("App") ?? throw new InvalidOperationException("App Connection string 'DefaultConnection' not found.");

        return services
            .AddScoped<Service.IRepository, Repository>()
            .AddScoped<Repository.IClient, Repository.Client>()
            .AddDbContext<AppDB>((sp, options) => options.EnableDetailedErrors()
                                                         .EnableSensitiveDataLogging()
                                                         .UseSqlServer(connectionString));

    }
}
