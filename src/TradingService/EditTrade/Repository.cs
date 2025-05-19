using Domain;
using Infrastructure.Technology.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TradingService.EditTrade;

internal class RepositoryAdapter(IRepository repository) : IRepositoryAdapter {

    public async Task<Trade> Edit(EditTradeRequest request, CancellationToken token) {

        var dataModel = await repository.FindById(request.TransactionId, token);
        dataModel!.Instrument = request.Name;
        await repository.Update(dataModel, token);

        var domainModel = MakeItEntityFrameworkFree(dataModel);
        return domainModel;
    }

    private static Trade MakeItEntityFrameworkFree(Infrastructure.Adapters.App.Data.Model.Trade dataModel) => new(
            traderId: dataModel.Id.ToString(),
            instrument: dataModel.Instrument,
            side: TradeSide.Buy,
            price: 0,
            quantity: 0,
            orderType: OrderType.Market,
            timeInForce: TimeInForce.Day,
            strategyCode: null,
            portfolioCode: null,
            userComment: null,
            executionRequestedForUtc: null);
}

public interface IRepository {
    Task<Infrastructure.Adapters.App.Data.Model.Trade?> FindById(Guid id, CancellationToken token);
    Task<Infrastructure.Adapters.App.Data.Model.Trade?> FindByName(string name, CancellationToken token);
    Task Update(Infrastructure.Adapters.App.Data.Model.Trade model, CancellationToken token);
}

public class Repository(AppDB db) : IRepository {

    public Task<Infrastructure.Adapters.App.Data.Model.Trade?> FindById(Guid id, CancellationToken token) =>
        db.Trades.FirstOrDefaultAsync(x => x.Id == id, token);

    public Task<Infrastructure.Adapters.App.Data.Model.Trade?> FindByName(string name, CancellationToken token) =>
        db.Trades.FirstOrDefaultAsync(x => x.Instrument == name, token);

    public async Task Update(Infrastructure.Adapters.App.Data.Model.Trade model, CancellationToken token) {
        db.Update(model);
        await db.SaveChangesAsync(token);
    }
}

public static class RepositoryExtensions {
    public static IServiceCollection AddRepository(this IServiceCollection services) => services
        .AddScoped<IRepositoryAdapter, RepositoryAdapter>()
        .AddScoped<IRepository, Repository>();
}