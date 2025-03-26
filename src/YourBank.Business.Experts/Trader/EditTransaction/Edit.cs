using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Business.Domain;
using Infrastructure.Adapters.App.Data.Model;
using Infrastructure.Technology.EF.App;

namespace Business.Experts.Trader.EditTransaction;

public class Edit {
    public class Business(Business.IInfrastructure client) : Feature.IEdit {
        public async Task<bool> Run(Feature.Response response) {

            var dataModelToUpdate = await client.FindById(response.Request.TransactionId, response.Token);
            SetDtaModel(dataModelToUpdate, response.Request);
            var updatedDataModel = await client.Update(dataModelToUpdate, response.Token);
            var businessModel = ToBusinessModel(updatedDataModel);
            response.Transaction = businessModel;
            return businessModel != null;
        }

        private void SetDtaModel(Transaction dm, Feature.Request request) {
            dm.Name = request.Name;
        }

        private Trade ToBusinessModel(Transaction dataModel) => new() {
            Id = dataModel.Id,
            Name = dataModel.Name
        };



    }

    public class Adapter(Adapter.IInfrastructure infrastructure) {

        public async Task<bool> Edit(Feature.Response response) {

            var dataModelToUpdate = await infrastructure.FindById(response.Request.TransactionId, response.Token);
            SetDtaModel(dataModelToUpdate, response.Request);
            var updatedDataModel = await infrastructure.Update(dataModelToUpdate, response.Token);
            var businessModel = ToBusinessModel(updatedDataModel);
            response.Transaction = businessModel;
            return businessModel != null;
        }

        private void SetDtaModel(Transaction dm, Feature.Request request) {
            dm.Name = request.Name;
        }

        private Trade ToBusinessModel(Transaction dataModel) => new() {
            Id = dataModel.Id,
            Name = dataModel.Name
        };

        public interface IInfrastructure {
            Task<Transaction?> FindById(long id, CancellationToken token);
            Task<Transaction?> FindByName(string name, CancellationToken token);
            Task<Transaction> Update(Transaction model, CancellationToken token);
        }
    }

    public class Infrastructure(AppDB db) : Adapter.IInfrastructure {

        public Task<Transaction?> FindById(long id, CancellationToken token) =>
            db.Transactions.FirstOrDefaultAsync(x => x.Id == id, token);

        public Task<Transaction?> FindByName(string name, CancellationToken token) =>
            db.Transactions.FirstOrDefaultAsync(x => x.Name == name, token);

        public async Task<Transaction> Update(Transaction model, CancellationToken token) {
            db.Update(model);
            await db.SaveChangesAsync(token);
            return model;
        }
    }
}

public static class EditExtensions {
    public static IServiceCollection AddEdit(this IServiceCollection services) => services
        .AddScoped<Feature.IEdit, Edit.Business>()
        .AddScoped<Edit.Adapter.IInfrastructure, Edit.Infrastructure>();
}