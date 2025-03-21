using Business.Domain;
using Business.Experts.Trader.EditTransaction;
using FluentValidation;
using Infrastructure.Adapters.App.Data.Model;
using Infrastructure.Technology.EF.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Experts.Trader.FindTransactions;




public class Repository {
    public class Adapter(Adapter.IInfrastructure client) : Feature.IRepository {
        public async Task<List<Trade>> FindTransactions(Featrure.Request request, CancellationToken token) {
            var dataModel = await client.Find(request.Name, token);
            var businessModel = dataModel.Select(ToBusinessModel).ToList();

            token.ThrowIfCancellationRequested();
            return businessModel;
        }

        private static List<Trade> ToBusinessModelList(List<TransactionDM> dataModelList) => dataModelList.Select(ToBusinessModel).ToList();
        private static Trade ToBusinessModel(TransactionDM dataModel) => new() {
            Id = dataModel.Id,
            Name = dataModel.Name
        };


        public interface IInfrastructure {
            public Task<List<TransactionDM>> Find(string? name, CancellationToken token);
        }
    }

    public class Infrastructure(AppDB db) : Adapter.IInfrastructure {

        public async Task<List<TransactionDM>> Find(string? name, CancellationToken token) {
            token.ThrowIfCancellationRequested();

            var transactions = name == null ?
               await db.Transactions.AsNoTracking().ToListAsync(token) :
               await db.Transactions.AsNoTracking().Where(x => x.Name.Contains(name)).ToListAsync(token);

            token.ThrowIfCancellationRequested();

            return transactions;
        }
    }
}
public static class RepositoryExtensions {
    public static IServiceCollection AddRepository(this IServiceCollection services) => services
        .AddScoped<Feature.IRepository, Repository.Adapter>()
        .AddScoped<Repository.Adapter.IInfrastructure, Repository.Infrastructure>();
}