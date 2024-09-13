using Common.Adapters.App.Data.Model;
using Common.Business.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions.Repository.EntityFramework;

public class Adapter(Adapter.IClient client) : Service.IRepository
{
    public async Task<List<Transaction>> FindTransactions(Service.Request request, CancellationToken token)
    {
        var dataModelList = await client.Find(request.Name, token);
        var businessModelList = dataModelList.Select(ToBusinessModel).ToList();
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
}

public static class AdapterExtensions {
    public static IServiceCollection AddRepositoryAdapter(this IServiceCollection services, ConfigurationManager configuration) => services
        .AddScoped<Service.IRepository, Adapter>()
        .AddRepositoryClient(configuration);
}
