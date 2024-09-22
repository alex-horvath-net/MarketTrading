using Common.Adapters.App.Data.Model;
using Common.Technology.EF.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.EditTransaction.Repository.EntityFramework;

public class Client(AppDB db) : Adapter.IClient {

    public async Task<TransactionDM> FindById(long id, CancellationToken token) => await db.FindAsync<TransactionDM>(id, token) ?? 
        throw new ArgumentException("Transaction not found");

    public Task<bool> ExistsById(long id, CancellationToken token) =>
       db.Transactions.AnyAsync(x => x.Id == id, token);

    public Task<bool> NameIsUnique(string name, CancellationToken token) =>
        db.Transactions.AllAsync(x => x.Name != name, token);

    public async Task<TransactionDM> Update(TransactionDM model, CancellationToken token) {
        db.Update<TransactionDM>(model);
        await db.SaveChangesAsync(token);
        return model;
    }
}

public static class ClientExtensions {
    public static IServiceCollection AddRepositoryClient(this IServiceCollection services, ConfigurationManager configuration) => services
        .AddScoped<Adapter.IClient, Client>()
        .AddRepositoryTechnology(configuration);
}
