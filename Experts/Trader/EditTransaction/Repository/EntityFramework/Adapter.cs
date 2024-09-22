using Common.Adapters.App.Data.Model;
using Common.Business.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.EditTransaction.Repository.EntityFramework;

public class Adapter(Adapter.IClient client) : Service.IRepository {
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
}


public static class AdapterExtensions {
    public static IServiceCollection AddRepositoryAdapter(this IServiceCollection services, ConfigurationManager configuration) => services
        .AddScoped<Service.IRepository, Adapter>()
        .AddRepositoryClient(configuration);
}