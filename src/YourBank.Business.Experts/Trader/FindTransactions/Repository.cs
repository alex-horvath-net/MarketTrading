using Business.Domain;
using FluentValidation;
using Infrastructure.Adapters.App.Data.Model;
using Infrastructure.Technology.EF.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Experts.Trader.FindTransactions;

internal class RepositoryAdapter(IRepository repository) : IRepositoryAdapter {
    public async Task<List<Trade>> Find(FindTransactionsRequest request, CancellationToken token) {
        var dataModel = request.Name == null ?
            await repository.FindAll(token) :
            await repository.FindByName(request.Name, token);
        var domainModel = dataModel.Select(MakeItEntityFrameworkFree).ToList();
        return domainModel;
    }

    private static Trade MakeItEntityFrameworkFree(Transaction dataModel) =>
        new() { Id = dataModel.Id, Name = dataModel.Name };
}

internal interface IRepository {
    Task<List<Transaction>> FindAll(CancellationToken token);
    Task<List<Transaction>> FindByName(string name, CancellationToken token);
}

internal class Repository(AppDB db) : IRepository {
    public async Task<List<Transaction>> FindAll(CancellationToken token) => await db
        .Transactions
        .AsNoTracking()
        .ToListAsync(token);

    public async Task<List<Transaction>> FindByName(string name, CancellationToken token) => await db
        .Transactions
        .AsNoTracking()
        .Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
        .ToListAsync(token);
}

public static class RepositoryExtensions {
    public static IServiceCollection AddRepository(this IServiceCollection services) => services
        .AddScoped<IRepositoryAdapter, RepositoryAdapter>()
        .AddScoped<IRepository, Repository>();
}