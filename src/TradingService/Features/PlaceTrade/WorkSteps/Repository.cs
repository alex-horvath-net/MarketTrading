using System.Diagnostics;
using IdentityService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TradingService.Domain;

namespace TradingService.Features.PlaceTrade.WorkSteps;

internal class RepositoryAdapter(IRepository repository) : IRepositoryAdapter {
    public async Task<Trade> Create(PlaceTradeRequest request, CancellationToken token) {
        var tradeDataModel = await repository.Create(token);
        var tradeDomainModel = MakeItEntityFrameworkFree(tradeDataModel);
        return tradeDomainModel;
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

internal interface IRepository {
    Task<Infrastructure.Adapters.App.Data.Model.Trade> Create(CancellationToken token);
}

internal class Repository(AppDB db) : IRepository {
    public async Task<Infrastructure.Adapters.App.Data.Model.Trade> Create(CancellationToken token) {
        var dataModel = new Infrastructure.Adapters.App.Data.Model.Trade();
        db.Trades.Add(dataModel);
        await db.SaveChangesAsync(token);
        return dataModel;
    }
}

internal class RepositoryCacheDecorator : IRepository {
    private readonly IRepository _repository;
    private readonly IMemoryCache _cache;
    private readonly MemoryCacheEntryOptions _cacheOptions;

    public RepositoryCacheDecorator(IRepository repository, IMemoryCache cache) {
        _repository = repository;
        _cache = cache;
        _cacheOptions = new MemoryCacheEntryOptions()
            .SetPriority(CacheItemPriority.High)
            .SetSlidingExpiration(TimeSpan.FromMinutes(2))
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
    }

    public async Task<Infrastructure.Adapters.App.Data.Model.Trade> Create(CancellationToken token) {
            return await _repository.Create(token);
    }

}

internal class RepositoryMeasureDecorator : IRepository {
    private readonly IRepository _repository;
    private readonly ILogger<RepositoryMeasureDecorator> _logger;


    public RepositoryMeasureDecorator(IRepository repository, ILogger<RepositoryMeasureDecorator> logger) {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Infrastructure.Adapters.App.Data.Model.Trade> Create(CancellationToken token) {
        var stopwatch = Stopwatch.StartNew();
        var result = await _repository.Create(token);
        stopwatch.Stop();
        _logger.LogInformation($"Create executed in {stopwatch.ElapsedMilliseconds} ms");
        return result;
    }
}

internal static class RepositoryExtensions {
    public static IServiceCollection AddRepository(this IServiceCollection services, ConfigurationManager config) => services
        .AddScoped<IRepositoryAdapter, RepositoryAdapter>()
        .AddScoped<IRepository, Repository>()
        //.AddMemoryCache()
        //.AddScoped<IRepository>(provider =>
        //    new RepositoryCacheDecorator(
        //        provider.GetRequiredService<Repository>(),
        //        provider.GetRequiredService<IMemoryCache>()
        //    ))
        .AddDbContext<AppDB>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
}
