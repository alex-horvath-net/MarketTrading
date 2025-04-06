using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Business.Domain;
using Infrastructure.Adapters.App.Data.Model;
using Infrastructure.Technology.EF.App;

namespace Business.Experts.Trader.EditTransaction;

internal class RepositoryAdapter(IRepository repository) : IRepositoryAdapter {

    public async Task<Trade> Edit(EditTransactionRequest request, CancellationToken token) {

        var dataModel = await repository.FindById(request.TransactionId, token);
        dataModel!.Name = request.Name;
        await repository.Update(dataModel, token);

        var domainModel = ToDomainModel(dataModel);
        return domainModel;
    }

    private static Trade ToDomainModel(Transaction? dataModel) {
        return new Trade() {
            Id = dataModel.Id,
            Name = dataModel.Name
        };
    }
}

public interface IRepository {
    Task<Transaction?> FindById(long id, CancellationToken token);
    Task<Transaction?> FindByName(string name, CancellationToken token);
    Task Update(Transaction model, CancellationToken token);
}

public class Repository(AppDB db) : IRepository {

    public Task<Transaction?> FindById(long id, CancellationToken token) =>
        db.Transactions.FirstOrDefaultAsync(x => x.Id == id, token);

    public Task<Transaction?> FindByName(string name, CancellationToken token) =>
        db.Transactions.FirstOrDefaultAsync(x => x.Name == name, token);

    public async Task Update(Transaction model, CancellationToken token) {
        db.Update(model);
        await db.SaveChangesAsync(token);
    }
}

public static class RepositoryExtensions {
    public static IServiceCollection AddRepository(this IServiceCollection services) => services
        .AddScoped<IRepositoryAdapter, RepositoryAdapter>()
        .AddScoped<IRepository, Repository>();
}