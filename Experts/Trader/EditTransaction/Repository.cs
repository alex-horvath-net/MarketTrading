using Common.Adapters.App.Data.Model;
using Common.Business.Model;
using Common.Technology.EF.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.EditTransaction;

public class Repository(Repository.IClient client) : Service.IRepository {
    public async Task<Transaction> EditTransaction(Service.Request request, CancellationToken token) {
        var dataModelToUpdate = await client.FindById(request.TransactionId, token);
        SetDtaModel(dataModelToUpdate, request);
        var updatedDataModel = await client.Update(dataModelToUpdate, token);
        var businessModel = ToBusinessModel(updatedDataModel);
        return businessModel;
    }

    private void SetDtaModel(TransactionDM dm, Service.Request request) {
        dm.Name = request.Name;
    }

    private Transaction ToBusinessModel(TransactionDM dataModel) => new() {
        Id = dataModel.Id,
        Name = dataModel.Name
    };


    public interface IClient {
        Task<TransactionDM> FindById(long id, CancellationToken token);
        Task<bool> ExistsById(long id, CancellationToken token);
        Task<bool> NameIsUnique(string name, CancellationToken token);
        Task<TransactionDM> Update(TransactionDM model, CancellationToken token);
    }

    public class Client(AppDB db) : IClient {

        public async Task<TransactionDM> FindById(long id, CancellationToken token) => await db.FindAsync<TransactionDM>(id, token) ??
            throw new ArgumentException("Transaction not found");

        public Task<bool> ExistsById(long id, CancellationToken token) =>
           db.Transactions.AnyAsync(x => x.Id == id, token);

        public Task<bool> NameIsUnique(string name, CancellationToken token) =>
            db.Transactions.AllAsync(x => x.Name != name, token);

        public async Task<TransactionDM> Update(TransactionDM model, CancellationToken token) {
            db.Update(model);
            await db.SaveChangesAsync(token);
            return model;
        }
    }
}


public static class AdapterExtensions {
    public static IServiceCollection AddRepositoryAdapter(this IServiceCollection services, ConfigurationManager configuration) => services
        .AddScoped<Service.IRepository, Repository>()
        .AddRepositoryClient(configuration);

    public static IServiceCollection AddRepositoryClient(this IServiceCollection services, ConfigurationManager configuration) => services
       .AddScoped<Repository.IClient, Repository.Client>()
       .AddRepositoryTechnology(configuration);

    public static IServiceCollection AddRepositoryTechnology(this IServiceCollection services, ConfigurationManager configuration) => services
     .AddDbContext<AppDB>(builder => builder.UseSqlServer(configuration.GetConnectionString("App")));
}