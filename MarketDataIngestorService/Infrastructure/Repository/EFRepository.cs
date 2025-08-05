using MarketDataIngestionService.Features.IngestLiveMarketData;
using MarketDataIngestionService.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace MarketDataIngestionService.Infrastructure.Repository;
public class EFRepository : IRepository {
    private readonly IngestorDbContext _db;
    private readonly ILogger<EFRepository> _logger;
    private readonly ResiliencePipeline _resiliencePipeline;

    public EFRepository(IngestorDbContext db, ILogger<EFRepository> logger) {
        _db = db;
        _logger = logger;
        _resiliencePipeline = new ResiliencePipelineBuilder()
          .AddRetry(new RetryStrategyOptions {
              MaxRetryAttempts = 3,
              Delay = TimeSpan.FromMilliseconds(500),
              BackoffType = DelayBackoffType.Exponential,
              OnRetry = args => {
                  logger.LogWarning(args.Outcome.Exception, "Retry #{Retry} after {Delay}ms", args.AttemptNumber, args.RetryDelay.TotalMilliseconds);
                  return ValueTask.CompletedTask;
              }
          })
          .AddCircuitBreaker(new CircuitBreakerStrategyOptions {
              ShouldHandle = args => PredicateResult.True(),
              FailureRatio = 1.0,
              MinimumThroughput = 5,
              SamplingDuration = TimeSpan.FromSeconds(10),
              BreakDuration = TimeSpan.FromSeconds(30),
              OnOpened = args => {
                  logger.LogError(args.Outcome.Exception, "Circuit breaker opened for {Duration} seconds", args.BreakDuration.TotalSeconds);
                  return ValueTask.CompletedTask;
              },
              OnClosed = _ => {
                  logger.LogInformation("Circuit breaker reset");
                  return ValueTask.CompletedTask;
              }
          })
          .Build();
    }

    public async Task<IEnumerable<string>> LoadSymbols(CancellationToken token) {
        var symbols = await _resiliencePipeline.ExecuteAsync(
            async (t) => await LoadSymbolsCore(t),
            token
            );

        _logger.LogDebug("Symbols are loaded");
        return symbols;
    }

    private async Task<IEnumerable<string>> LoadSymbolsCore(CancellationToken token) => await _db.Symbols
                .Where(s => s.IsActive)
                .Where(s => !string.IsNullOrWhiteSpace(s.Name))
                .Select(s => s.Name)
                .ToListAsync(token);
}
