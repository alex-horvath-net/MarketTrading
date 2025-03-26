using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Business.Domain;
using Infrastructure.Adapters.App.Data.Model;
using Infrastructure.Technology.EF.App;

namespace Business.Experts.Trader.EditTransaction;

public class Edit {
    public class Business(Business.IAdapter adapter) : Feature.IEdit {
        public async Task<bool> Run(Feature.Response response) {
            response.Transaction = await adapter.Edit(response.Request, response.Token);
            return response.Transaction != null;
        }

        public interface IAdapter {
            Task<Domain.Trade> Edit(Feature.Request request, CancellationToken token);
        }
    }

    public class Adapter(Adapter.IInfrastructure infrastructure) : Business.IAdapter {

        public async Task<Trade> Edit(Feature.Request request, CancellationToken token) {

            var infraModel = await infrastructure.FindById(request.TransactionId, token);
            infraModel!.Name = request.Name;
            var updatedInfraModel = await infrastructure.Update(infraModel, token);

            var businessModel = ToBusinessModel(updatedInfraModel);
            return businessModel;
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

        // InfraModel is Infrastructure.Adapters.App.Data.Model.Transaction
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