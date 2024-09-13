using Common.Adapters.App.Data.Model;
using Common.Technology.EF.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions.Repository.EntityFramework;

public class Client(AppDB db) : Adapter.IClient {
    public Task<List<TransactionDM>> Find(string? name, CancellationToken token) =>
        name == null ?
        db.Transactions.ToListAsync(token) :
        db.Transactions.Where(x => x.Name == name).ToListAsync(token);
}

public static class ClientExtensions {
    public static IServiceCollection AddRepositoryClient(this IServiceCollection services, ConfigurationManager configuration) => services
        .AddScoped<Adapter.IClient, Client>()
        .AddDbContext<Common.Technology.EF.App.AppDB>(builder => builder.UseSqlServer(configuration.GetConnectionString("App")));
}
  