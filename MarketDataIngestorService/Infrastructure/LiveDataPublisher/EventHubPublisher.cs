using System.Text.Json;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using MarketDataIngestionService.Domain;
using MarketDataIngestionService.Features.IngestLiveMarketData;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace MarketDataIngestionService.Infrastructure.LiveDataPublisher;

public class EventHubPublisher : IPublisher {
    private readonly EventHubProducerClient _eventHub;
    private readonly ResiliencePipeline _resiliencePipeline;
    private readonly IBuffer _buffer;
    private readonly IListBacth _batch;
    private readonly ITime _time;
    private readonly ILogger<EventHubPublisher> _logger;
    private long _publishedCount;
    private long _skippedCount;

    public EventHubPublisher(
        EventHubProducerClient eventHub,
        IBuffer buffer,
        IListBacth batch,
        ITime time,
        ILogger<EventHubPublisher> logger,
        ResiliencePipeline? resiliencePipeline = null) {
        _eventHub = eventHub ?? throw new ArgumentNullException(nameof(eventHub));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
        _batch = batch ?? throw new ArgumentNullException(nameof(batch));
        _time = time ?? throw new ArgumentNullException(nameof(time));
        _resiliencePipeline = resiliencePipeline ?? BuildDefaultResiliencePipeline(_logger);
    }

    public async Task PublishLiveData(string hostId, CancellationToken token) {
        var batch = new List<MarketPrice>();

        await foreach (var liveData in _buffer.GetItemsAsync(token)) {
            token.ThrowIfCancellationRequested();
            if (_batch.Add(liveData, ref batch)) {
                await PublishListBatch(batch, hostId, token);
            }
        }

        if (_batch.Count > 0) {
            await PublishListBatch(batch, hostId, token);
        }
    }

    private async Task PublishListBatch(List<MarketPrice> listBatch, string hostId, CancellationToken token) {
        var grouped = listBatch.GroupBy(p => p.Symbol.Replace(" ", "").ToUpperInvariant());
        foreach (var group in grouped) {
            var symbol = group.Key;
            var events = group.ToList();
            try {
                await Publish(events, symbol, token);
            } catch (Exception ex) {
                _logger.LogError(ex, $"Failed to publish {events.Count} events to partition {symbol}");
            }
        }
        listBatch.Clear();
    }

    private async Task Publish(IEnumerable<MarketPrice> batch, string partitionKey, CancellationToken token) {
        var batchOptions = new CreateBatchOptions { PartitionKey = partitionKey };
        var eventBatch = await _eventHub.CreateBatchAsync(batchOptions, token);

        int skipped = 0;
        foreach (var liveData in batch) {
            var json = JsonSerializer.Serialize(liveData);
            var eventData = new EventData(json) { CorrelationId = liveData.CorrelationId };
            eventData.Properties[nameof(liveData.Symbol)] = liveData.Symbol;
            eventData.Properties[nameof(liveData.Timestamp)] = liveData.Timestamp.ToString("O");

            if (!eventBatch.TryAdd(eventData)) {
                skipped = batch.Count() - eventBatch.Count;
                break;
            }
        }

        await _resiliencePipeline.ExecuteAsync(
            async t => await _eventHub.SendAsync(eventBatch, t),
            token
        );

        Interlocked.Add(ref _publishedCount, eventBatch.Count);
        Interlocked.Add(ref _skippedCount, skipped);
    }

    private static ResiliencePipeline BuildDefaultResiliencePipeline(ILogger logger) {
        return new ResiliencePipelineBuilder()
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
}