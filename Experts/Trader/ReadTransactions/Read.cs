using Common.Adapters.AppDataModel;
using Common.Business.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.ReadTransactions;


public class RepositoryAdapterPlugin(
    RepositoryAdapterPlugin.RepositoryTechnologyPort repositoryTechnology) :Business.IRepositoryAdapterPort {
    public async Task<List<TransactionBM>> ReadTransaction(
        Business.Request request, CancellationToken token) {
        var adapterData = request.Name == null ?
            await repositoryTechnology.ReadTransaction(token) :
            await repositoryTechnology.ReadTransaction(request.Name, token);

        var businessData = adapterData.Select(ToBusinessData).ToList();
        return businessData;
    }

    private TransactionBM ToBusinessData(Common.Adapters.AppDataModel.Transaction data) =>
        new() {
            Id = data.Id,
            Name = data.Name
        };

    public interface RepositoryTechnologyPort {
        public Task<List<Transaction>> ReadTransaction(CancellationToken token);
        public Task<List<Common.Adapters.AppDataModel.Transaction>> ReadTransaction(string name, CancellationToken token);
    }
}

public class RepositoryTechnologyPlugin(Common.Technology.AppData.AppDB db) : RepositoryAdapterPlugin.RepositoryTechnologyPort {

    public async Task<List<Common.Adapters.AppDataModel.Transaction>> ReadTransaction(
        CancellationToken token) => await db.Transactions
                .ToListAsync(token);

    public async Task<List<Common.Adapters.AppDataModel.Transaction>> ReadTransaction(
        string name,
        CancellationToken token) => await db.Transactions
                .Where(x => x.Name.Contains(name))
                .ToListAsync(token);
}

public static class ReadExtensions {
    public static IServiceCollection AddRead(this IServiceCollection services, ConfigurationManager configuration) {
        services.AddScoped<Business.IRepositoryAdapterPort, RepositoryAdapterPlugin>()
            .AddScoped<RepositoryAdapterPlugin.RepositoryTechnologyPort, RepositoryTechnologyPlugin>()
                .AddDbContext<Common.Technology.AppData.AppDB>(builder => builder.UseSqlServer(configuration.GetConnectionString("App")));

        return services;
    }
}
