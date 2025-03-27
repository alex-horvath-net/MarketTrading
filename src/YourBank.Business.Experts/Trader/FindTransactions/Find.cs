using Business.Domain;
using FluentValidation;
using Infrastructure.Adapters.App.Data.Model;
using Infrastructure.Technology.EF.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
//using static Business.Experts.Trader.FindTransactions.Feature;

namespace Business.Experts.Trader.FindTransactions;
public class Find {
    public class Business(Business.IAdapter adapter) : Feature.IFind {

        public async Task<bool> Run(Feature.Response response) {
            response.Transactions = await adapter.Find(response.Request, response.Token);
            return !response.Transactions.Any();
        }

        public interface IAdapter {
            public Task<List<Trade>> Find(Feature.Request request, CancellationToken token);
        }
    }

    public class Adapter(Adapter.IInfrastructure infrastructure) : Business.IAdapter {

        public async Task<List<Trade>> Find(Feature.Request request, CancellationToken token) {
            var infraModel = await infrastructure.Find(request.Name, token);

            var businessModel = infraModel
                .Select(model => new Trade() { Id = model.Id, Name = model.Name })
                .ToList();

            return businessModel;
        }

        public interface IInfrastructure {
            Task<List<Transaction>> Find(string? name, CancellationToken token);
        }

    }

    public class Infrastructure(AppDB db) : Adapter.IInfrastructure {
        public async Task<List<Transaction>> Find(string? name, CancellationToken token) {
            token.ThrowIfCancellationRequested();

            var query = db.Transactions.AsNoTracking();

            if (name != null)
                query = query.Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

            return await query.ToListAsync(token);
        }

    }
}
public static class FindExtensions {
    public static IServiceCollection AddRepository(this IServiceCollection services) => services
        .AddScoped<Feature.IFind, Find.Business>()
        .AddScoped<Find.Business.IAdapter, Find.Adapter>();
}