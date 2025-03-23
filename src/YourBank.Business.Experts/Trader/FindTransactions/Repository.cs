using Business.Domain;
using FluentValidation;
using Infrastructure.Adapters.App.Data.Model;
using Infrastructure.Technology.EF.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static Business.Experts.Trader.FindTransactions.Feature;
using static Business.Experts.Trader.FindTransactions.Validate.Adapter;

namespace Business.Experts.Trader.FindTransactions;
public class Repository {
    public class Business(Business.IAdapter client) : IRepository {

        public async Task<List<Trade>> FindTransactions(Feature.Request request, CancellationToken token) {
            var dataModel = await client.Find(request.Name, token);
            var businessModel = dataModel.Select(ToBusinessModel).ToList();

            token.ThrowIfCancellationRequested();
            return businessModel;
        }

        private static List<Trade> ToBusinessModelList(List<Transaction> dataModelList) => dataModelList.Select(ToBusinessModel).ToList();
        private static Trade ToBusinessModel(Transaction dataModel) => new() {
            Id = dataModel.Id,
            Name = dataModel.Name
        };


        public interface IAdapter {
            public Task<List<Transaction>> Find(string? name, CancellationToken token);
        }
    }

    public class Adapter(AppDB infrastructure) : Business.IAdapter {

        public async Task<List<Transaction>> Find(string? name, CancellationToken token) {
            token.ThrowIfCancellationRequested();
            var dataModel = await client.Find(request.Name, token);
            var businessModel = dataModel.Select(ToBusinessModel).ToList();
        }

        public interface IInfrastructure {
            Task<List<InfraModel>> Find(Feature.Request request, CancellationToken token);
        }

    }

    public class Infrastructure(AppDB db) {
        public async Task<List<Transaction>> Find(string? name, CancellationToken token) {
            token.ThrowIfCancellationRequested();

            var query = db.Transactions.AsNoTracking();

            if (name != null)
                query = query.Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

            return await query.ToListAsync(token);
        }
    }
}
public static class RepositoryExtensions {
    public static IServiceCollection AddRepository(this IServiceCollection services) => services
        .AddScoped<IRepository, Repository.Business>()
        .AddScoped<Repository.Business.IAdapter, Repository.Adapter>();
}