using Common.Adapters.App.Data.Model;
using Common.Business.Model;
using Common.Technology.EF.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions;

public class Repository(Repository.IClient client) : Service.IRepository
{
    public async Task<List<Transaction>> FindTransactions(Request request, CancellationToken token)
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
    public class Client(AppDB db) : IClient
    {
        public async Task<List<TransactionDM>> Find(string? name, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var transactions = name == null ?
               await db.Transactions.ToListAsync(token) :
               await db.Transactions.Where(x => x.Name.Contains(name)).ToListAsync(token);

            token.ThrowIfCancellationRequested();
            return transactions;
        }
    }

}

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepository(this IServiceCollection services, ConfigurationManager configuration) => services
        .AddScoped<Service.IRepository, Repository>()
        .AddRepositoryClient(configuration);

    public static IServiceCollection AddRepositoryClient(this IServiceCollection services, ConfigurationManager configuration) => services
        .AddScoped<Repository.IClient, Repository.Client>()
        .AddRepositoryTechnology(configuration);

    public static IServiceCollection AddRepositoryTechnology(this IServiceCollection services, ConfigurationManager configuration) => services
       .AddDbContext<AppDB>(builder => builder.UseSqlServer(configuration.GetConnectionString("App")));


}
