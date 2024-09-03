using Common.Technology.AppData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.ReadTransactions;


public class RepositoryAdapterPlugin(
    RepositoryAdapterPlugin.RepositoryTechnologyPort repositoryTechnology) :
    Feature.IRepositoryAdapterPort {
    public async Task<List<Common.Business.Transaction>> ReadTransaction(
        Feature.Request request,
        CancellationToken token) {
        var adapterData = await repositoryTechnology.ReadTransaction(request.Name, token);
        var businessData = adapterData.Select(ToDomain).ToList();
        return businessData;
    }

    private Common.Business.Transaction ToDomain(Common.Adapters.AppDataModel.Transaction data) =>
        new() {
            Id = data.Id,
            Name = data.Name
        };

    public interface RepositoryTechnologyPort {
        public Task<List<Common.Adapters.AppDataModel.Transaction>> ReadTransaction(
            string name,
            CancellationToken token);
    }
}

public class RepositoryTechnologyPlugin(
    Common.Technology.AppData.AppDB db) :
    RepositoryAdapterPlugin.RepositoryTechnologyPort {
    public async Task<List<Common.Adapters.AppDataModel.Transaction>> ReadTransaction(
        string name,
        CancellationToken token) {
        try {
            var v = await db
                .Transactions
                .Where(x => x.Name.Contains(name))
                .ToListAsync(token);
            return v;
        } catch (Exception ex) {
            throw;
        }
    }
}

public static class ReadExtensions {
    public static IServiceCollection AddRead(this IServiceCollection services, ConfigurationManager configuration) {
        services.AddScoped<Feature.IRepositoryAdapterPort, RepositoryAdapterPlugin>()
            .AddScoped<RepositoryAdapterPlugin.RepositoryTechnologyPort, RepositoryTechnologyPlugin>()
                .AddDbContext<AppDB>(builder => builder.UseSqlServer(configuration.GetConnectionString("App")));

        return services;
    }
}
