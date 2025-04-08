using System.Diagnostics;
using Business.Domain;
using FluentValidation;
using Infrastructure.Adapters.App.Data.Model;
using Infrastructure.Technology.EF.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Business.Experts.Trader.FindTransactions;

internal class RepositoryAdapter(IRepository repository) : IRepositoryAdapter {
    public async Task<List<Trade>> Find(FindTransactionsRequest request, CancellationToken token) {
        var dataModel = string.IsNullOrEmpty(request.TransactionName) ?
            await repository.FindAll(token) :
            await repository.FindByName(request.TransactionName, token);
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

    public async Task<List<Transaction>> FindAll(CancellationToken token) {
        var items = await _cache.GetOrCreateAsync($"FindAll", entry => {
            entry.SetOptions(_cacheOptions);
            return _repository.FindAll(token);
        });
        return items ?? [];
    }



    public async Task<List<Transaction>> FindByName(string name, CancellationToken token) {
        var items = await _cache.GetOrCreateAsync($"FindByName_{name}", async entry => {
            entry.SetOptions(_cacheOptions);
            return await _repository.FindByName(name, token);
        });
        return items ?? [];
    }
}


internal class RepositoryMeasureDecorator : IRepository {
    private readonly IRepository _repository;
    private readonly ILogger<RepositoryMeasureDecorator> _logger;


    public RepositoryMeasureDecorator(IRepository repository, ILogger<RepositoryMeasureDecorator> logger) {
        _repository = repository;
        _logger = logger;
    }

    public async Task<List<Transaction>> FindAll(CancellationToken token) {
        var stopwatch = Stopwatch.StartNew();
        var result = await _repository.FindAll(token);
        stopwatch.Stop();
        _logger.LogInformation($"FindAll executed in {stopwatch.ElapsedMilliseconds} ms");
        return result;
    }

    public async Task<List<Transaction>> FindByName(string name, CancellationToken token) {
        var stopwatch = Stopwatch.StartNew();
        var result = await _repository.FindByName(name, token);
        stopwatch.Stop();
        Console.WriteLine($"FindByName executed in {stopwatch.ElapsedMilliseconds} ms");
        return result;
    }
}

public static class RepositoryExtensions {
    public static IServiceCollection AddRepository(this IServiceCollection services, ConfigurationManager config) => services
        .AddScoped<IRepositoryAdapter, RepositoryAdapter>()
        .AddScoped<Repository>()
        .AddMemoryCache()
        .AddScoped<IRepository>(provider =>
            new RepositoryCacheDecorator(
                provider.GetRequiredService<Repository>(),
                provider.GetRequiredService<IMemoryCache>()
            ))
        .AddDbContext<AppDB>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
}
