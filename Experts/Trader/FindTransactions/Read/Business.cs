using Common.Adapters.AppDataModel;
using Common.Business.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions.Read;

public class Business(IRepository repository) {
    public async Task<List<Transaction>> ReadTransaction(Request request, CancellationToken token) {
        var technologyModelList = request.Name == null ? await repository.ReadTransaction(token) : await repository.ReadTransaction(request.Name, token);
        var businessModelList = technologyModelList.Select(technologyModel => new Transaction() {
            Id = technologyModel.Id,
            Name = technologyModel.Name
        }).ToList();
        return businessModelList;
    }
}


public interface IRepository {
    public Task<List<TransactionDM>> ReadTransaction(CancellationToken token);
    public Task<List<TransactionDM>> ReadTransaction(string name, CancellationToken token);
}


public class EntityFrameworkClient(EntityFramework ef) : IRepository {

    public async Task<List<TransactionDM>> ReadTransaction(CancellationToken token) =>
        await ef.Transactions.ToListAsync(token);

    public async Task<List<TransactionDM>> ReadTransaction(string name, CancellationToken token) =>
        await ef.Transactions.Where(x => x.Name.Contains(name)).ToListAsync(token);
}


public class EntityFramework : Common.Technology.AppData.AppDB {
}


public static class Extensions {
    public static IServiceCollection AddRead(this IServiceCollection services, ConfigurationManager configuration) {
        services.AddScoped<Business>()
            .AddScoped<IRepository, EntityFrameworkClient>()
                .AddDbContext<Common.Technology.AppData.AppDB>(builder => builder.UseSqlServer(configuration.GetConnectionString("App")));

        return services;
    }
}


