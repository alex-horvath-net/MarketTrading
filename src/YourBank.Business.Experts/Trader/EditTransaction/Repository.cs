using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Business.Domain;
using Infrastructure.Adapters.App.Data.Model;
using Infrastructure.Technology.EF.App;

namespace Business.Experts.Trader.EditTransaction;


internal class RepositoryAdapter(IRepository infrastructure) : IRepositoryAdapter {

    public async Task<Trade> Edit(EditTransactionRequest request, CancellationToken token) {

        var infraModel = await infrastructure.FindById(request.TransactionId, token);
        infraModel!.Name = request.Name;
        var updatedInfraModel = await infrastructure.Update(infraModel, token);

        var businessModel = new Trade() {
            Id = updatedInfraModel.Id,
            Name = updatedInfraModel.Name
        };
        return businessModel;
    }

}

public interface IRepository {
    Task<Transaction?> FindById(long id, CancellationToken token);
    Task<Transaction?> FindByName(string name, CancellationToken token);
    Task<Transaction> Update(Transaction model, CancellationToken token);
}

public class Repository(AppDB db) : IRepository {

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


public static class EditExtensions {
    public static IServiceCollection AddRepository(this IServiceCollection services) => services
        .AddScoped<IRepositoryAdapter, RepositoryAdapter>()
        .AddScoped<IRepository, Repository>();
}