using System.Diagnostics;
using FluentValidation;
using Infrastructure.Technology.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DataModel = Infrastructure.Adapters.App.Data.Model;

namespace Business.Experts.Trader.FindTrades;

internal class RepositoryAdapter(IRepository repository) : IRepositoryAdapter {
    public async Task<List<Domain.Trade>> Find(FindTradesRequest request, CancellationToken token) {
        var dataModel = await repository.FindWithFilters(
            request.Instrument,
            ToDataModelTradeSide(request.Side),
            request.FromDate,
            request.ToDate,
            token);
        var domainModel = dataModel.Select(MakeItEntityFrameworkFree).ToList();
        return domainModel;
    }

    private DataModel.TradeSide? ToDataModelTradeSide(string? side) => string.IsNullOrWhiteSpace(side) ? null : ToDataModelTradeSide(Enum.Parse<Domain.TradeSide>(side, true));

    private DataModel.TradeSide? ToDataModelTradeSide(Domain.TradeSide? side) => side switch {
        null => null,
        Domain.TradeSide.Buy => DataModel.TradeSide.Buy,
        Domain.TradeSide.Sell => DataModel.TradeSide.Sell,
        _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
    };

    private Domain.TradeSide ToDomainTradeSide(DataModel.TradeSide side) => side switch {
        DataModel.TradeSide.Buy => Domain.TradeSide.Buy,
        DataModel.TradeSide.Sell => Domain.TradeSide.Sell,
        _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
    };

    private Domain.Trade MakeItEntityFrameworkFree(DataModel.Trade dataModel) => new(
            traderId: dataModel.Id.ToString(),
            instrument: dataModel.Instrument,
            side: ToDomainTradeSide(dataModel.Side),
            price: 0,
            quantity: 0,
            orderType: Domain.OrderType.Market,
            timeInForce: Domain.TimeInForce.Day,
            strategyCode: null,
            portfolioCode: null,
            userComment: null,
            executionRequestedForUtc: null);
}

internal interface IRepository {
    Task<List<DataModel.Trade>> FindAll(CancellationToken token);
    Task<List<DataModel.Trade>> FindByName(string name, CancellationToken token);
    Task<List<DataModel.Trade>> FindWithFilters(string? instrument, DataModel.TradeSide? side, DateTime? fromDate, DateTime? toDate, CancellationToken token);

}

internal class Repository(AppDB db) : IRepository {
    public async Task<List<DataModel.Trade>> FindAll(CancellationToken token) => await db
        .Trades
        .AsNoTracking()
        .ToListAsync(token);

    public async Task<List<DataModel.Trade>> FindByName(string name, CancellationToken token) => await db
        .Trades
        .AsNoTracking()
        .Where(x => x.Instrument.Contains(name, StringComparison.OrdinalIgnoreCase))
        .ToListAsync(token);

    public async Task<List<DataModel.Trade>> FindWithFilters(string? instrument, DataModel.TradeSide? side, DateTime? fromDate, DateTime? toDate, CancellationToken token) {
        var query = db.Trades.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(instrument))
            query = query.Where(x => x.Instrument.Contains(instrument, StringComparison.OrdinalIgnoreCase));

        if (side.HasValue)
            query = query.Where(x => x.Side == side.Value);

        if (fromDate.HasValue)
            query = query.Where(x => x.SubmittedAt >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(x => x.SubmittedAt <= toDate.Value);

        return await query.ToListAsync(token);
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

    public async Task<List<DataModel.Trade>> FindAll(CancellationToken token) {
        var items = await _cache.GetOrCreateAsync($"FindAll", entry => {
            entry.SetOptions(_cacheOptions);
            return _repository.FindAll(token);
        });
        return items ?? [];
    }

    public async Task<List<DataModel.Trade>> FindByName(string name, CancellationToken token) {
        var items = await _cache.GetOrCreateAsync($"FindByName_{name}", async entry => {
            entry.SetOptions(_cacheOptions);
            return await _repository.FindByName(name, token);
        });
        return items ?? [];
    }

    public async Task<List<DataModel.Trade>> FindWithFilters(string? instrument, DataModel.TradeSide? side, DateTime? fromDate, DateTime? toDate, CancellationToken token) {
        return await _repository.FindWithFilters(instrument, side, fromDate, toDate, token);
    }
}


internal class RepositoryMeasureDecorator : IRepository {
    private readonly IRepository _repository;
    private readonly ILogger<RepositoryMeasureDecorator> _logger;


    public RepositoryMeasureDecorator(IRepository repository, ILogger<RepositoryMeasureDecorator> logger) {
        _repository = repository;
        _logger = logger;
    }

    public async Task<List<DataModel.Trade>> FindAll(CancellationToken token) {
        var stopwatch = Stopwatch.StartNew();
        var result = await _repository.FindAll(token);
        stopwatch.Stop();
        _logger.LogInformation($"FindAll executed in {stopwatch.ElapsedMilliseconds} ms");
        return result;
    }

    public async Task<List<DataModel.Trade>> FindByName(string name, CancellationToken token) {
        var stopwatch = Stopwatch.StartNew();
        var result = await _repository.FindByName(name, token);
        stopwatch.Stop();
        _logger.LogDebug($"FindByName executed in {stopwatch.ElapsedMilliseconds} ms");
        return result;
    }

    public async Task<List<DataModel.Trade>> FindWithFilters(string? instrument, DataModel.TradeSide? side, DateTime? fromDate, DateTime? toDate, CancellationToken token) {
        var stopwatch = Stopwatch.StartNew();
        var result = await _repository.FindWithFilters(instrument, side, fromDate, toDate, token);
        stopwatch.Stop();
        _logger.LogDebug($"FindWithFilters executed in {stopwatch.ElapsedMilliseconds} ms");
        return result;

    }
}

public static class RepositoryExtensions {
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
