using System.Text.Json;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using MarketDataIngestorService.Domain;
using MarketDataIngestorService.Features.LiveMarketData;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;

namespace MarketDataIngestorService.Infrastructure.LiveDataPublisher;

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

    public async Task SendAsync(MarketPrice price, CancellationToken token) {
        var json = JsonSerializer.Serialize(price);
        var eventData = new EventData(json) {
            CorrelationId = price.CorrelationId
        };

        eventData.Properties["Symbol"] = price.Symbol;
        eventData.Properties["Timestamp"] = price.Timestamp.ToString("O");


        var options = new SendEventOptions {
            PartitionKey = price.Symbol.Replace(" ", "").ToUpperInvariant()
        };


        var eventBatch = new List<EventData> { eventData };

        await _resiliencePipeline.ExecuteAsync(
            async (t) => await _eventHub.SendAsync(eventBatch, options, t),
            token
        );

        _logger.LogDebug("Sent event for {Symbol} [{CorrelationId}]", price.Symbol, price.CorrelationId);
    }
}
