using Common.Adapters.App.Data.Model;
using Common.Business.Model;
using Common.Technology.EF.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DomainExperts.Trader.EditTransaction.WorkSteps;

public class Repository {
    public class BusinessAdapter(BusinessAdapter.ITechnologyAdapter client) : BusinessNeed.IRepository {
        public async Task<Transaction> EditTransaction(BusinessNeed.Request request, CancellationToken token) {
            var dataModelToUpdate = await client.FindById(request.TransactionId, token);
            SetDtaModel(dataModelToUpdate, request);
            var updatedDataModel = await client.Update(dataModelToUpdate, token);
            var businessModel = ToBusinessModel(updatedDataModel);
            return businessModel;
        }

        private void SetDtaModel(TransactionDM dm, BusinessNeed.Request request) {
            dm.Name = request.Name;
        }

        private Transaction ToBusinessModel(TransactionDM dataModel) => new() {
            Id = dataModel.Id,
            Name = dataModel.Name
        };


        public interface ITechnologyAdapter {
            Task<TransactionDM> FindById(long id, CancellationToken token);
            Task<bool> ExistsById(long id, CancellationToken token);
            Task<bool> NameIsUnique(string name, CancellationToken token);
            Task<TransactionDM> Update(TransactionDM model, CancellationToken token);
        }
    }

    public class TechnologyAdapter(AppDB db) : BusinessAdapter.ITechnologyAdapter {

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
    public static IServiceCollection AddRepositoryAdapter(this IServiceCollection services) => services
        .AddScoped<BusinessNeed.IRepository, Repository.BusinessAdapter>()
        .AddScoped<Repository.BusinessAdapter.ITechnologyAdapter, Repository.TechnologyAdapter>();
}