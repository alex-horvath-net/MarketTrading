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
    private readonly ILogger<EventHubPublisher> _logger;
    private readonly ResiliencePipeline _resiliencePipeline;

    public EventHubPublisher(
        EventHubProducerClient eventHub,
        ILogger<EventHubPublisher> logger) {
        _eventHub = eventHub;
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

    public async Task Publish(IEnumerable<MarketPrice> liveDataBatch, string symbol, CancellationToken token) {
        var EventDataBatch = new List<EventData>();

        foreach (var liveData in liveDataBatch) {
            var json = JsonSerializer.Serialize(liveData);
            var eventData = new EventData(json) {
                CorrelationId = liveData.CorrelationId
            };
            eventData.Properties["Symbol"] = liveData.Symbol;
            eventData.Properties["Timestamp"] = liveData.Timestamp.ToString("O");
            
            EventDataBatch.Add(eventData);
        }

        var options = new SendEventOptions {
            PartitionKey = symbol.Replace(" ", "").ToUpperInvariant()
        };

        await _resiliencePipeline.ExecuteAsync(
            async (t) => await _eventHub.SendAsync(EventDataBatch, options, t),
            token
        );

        _logger.LogDebug("Published live data for {Symbol}", symbol);
    }
}
