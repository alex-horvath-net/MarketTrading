using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Business.Domain;
using Infrastructure.Adapters.App.Data.Model;
using Infrastructure.Technology.EF.App;

namespace Business.Experts.Trader.EditTransaction;

public class Repository {
    public class Adapter(Adapter.IInfrastructure client) : Feature.IRepository {
        public async Task<Trade> EditTransaction(Feature.Request request, CancellationToken token) {
            var dataModelToUpdate = await client.FindById(request.TransactionId, token);
            SetDtaModel(dataModelToUpdate, request);
            var updatedDataModel = await client.Update(dataModelToUpdate, token);
            var businessModel = ToBusinessModel(updatedDataModel);
            return businessModel;
        }

        private void SetDtaModel(TransactionDM dm, Feature.Request request) {
            dm.Name = request.Name;
        }

        private Trade ToBusinessModel(TransactionDM dataModel) => new() {
            Id = dataModel.Id,
            Name = dataModel.Name
        };


        public interface IInfrastructure {
            Task<TransactionDM> FindById(long id, CancellationToken token);
            Task<bool> ExistsById(long id, CancellationToken token);
            Task<bool> NameIsUnique(string name, CancellationToken token);
            Task<TransactionDM> Update(TransactionDM model, CancellationToken token);
        }
    }

    public class Infrastructure(AppDB db) : Adapter.IInfrastructure {

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
public static class RepositoryExtensions {
    public static IServiceCollection AddRepository(this IServiceCollection services) => services
        .AddScoped<Feature.IRepository, Repository.Adapter>()
        .AddScoped<Repository.Adapter.IInfrastructure, Repository.Infrastructure>();
}