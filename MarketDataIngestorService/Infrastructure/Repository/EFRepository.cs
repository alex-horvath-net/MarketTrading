using MarketDataIngestionService.Features.IngestLiveMarketData;
using MarketDataIngestionService.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace MarketDataIngestionService.Infrastructure.Repository;

public class EFRepository : IRepository
{
    private readonly IngestorDbContext _db;
    private readonly ILogger<EFRepository> _logger;
    private readonly ResiliencePipeline _resiliencePipeline;

    public EFRepository(
        IngestorDbContext db,
        ILogger<EFRepository> logger,
        ResiliencePipeline? resiliencePipeline = null)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _resiliencePipeline = resiliencePipeline ?? BuildDefaultResiliencePipeline(_logger);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<string>> LoadSymbols(CancellationToken token)
    {
        var symbols = await _resiliencePipeline.ExecuteAsync(
            async t => await QueryActiveSymbols(t),
            token);

        _logger.LogDebug("Symbols loaded: {Count}", symbols?.Count ?? 0);
        return symbols ?? Array.Empty<string>();
    }

    private async Task<IReadOnlyCollection<string>> QueryActiveSymbols(CancellationToken token)     
    {
        if (_db.Symbols == null)
        {
            _logger.LogWarning("Symbols DbSet is null");
            return Array.Empty<string>();
        }

        return await _db.Symbols
            .AsNoTracking()
            .Where(s => s.IsActive && !string.IsNullOrWhiteSpace(s.Name))
            .Select(s => s.Name)
            .ToListAsync(token);
    }

    private static ResiliencePipeline BuildDefaultResiliencePipeline(ILogger logger)
    {
        return new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromMilliseconds(500),
                BackoffType = DelayBackoffType.Exponential,
                OnRetry = args =>
                {
                    logger.LogWarning(args.Outcome.Exception, "Retry #{Retry} after {Delay}ms", args.AttemptNumber, args.RetryDelay.TotalMilliseconds);
                    return ValueTask.CompletedTask;
                }
            })
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                ShouldHandle = args => PredicateResult.True(),
                FailureRatio = 1.0,
                MinimumThroughput = 5,
                SamplingDuration = TimeSpan.FromSeconds(10),
                BreakDuration = TimeSpan.FromSeconds(30),
                OnOpened = args =>
                {
                    logger.LogError(args.Outcome.Exception, "Circuit breaker opened for {Duration} seconds", args.BreakDuration.TotalSeconds);
                    return ValueTask.CompletedTask;
                },
                OnClosed = _ =>
                {
                    logger.LogInformation("Circuit breaker reset");
                    return ValueTask.CompletedTask;
                }
            })
            .Build();
    }
}
