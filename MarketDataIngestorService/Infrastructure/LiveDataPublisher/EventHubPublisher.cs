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
    long _droppedCount = 0;

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

    public async Task StartPublishingLiveData(string hostId, CancellationToken token) {
        var listBatch = new List<MarketPrice>(200);
        DateTime batchStartTime = default;

        await foreach (var liveData in _buffer.GetItemsAsync(token)) {
            token.ThrowIfCancellationRequested();

            if (listBatch.Count == 0) {
                batchStartTime = _time.UtcNow;
            }

            listBatch.Add(liveData);

            bool batchReady = listBatch.Count >= 200 || (_time.UtcNow - batchStartTime).TotalMilliseconds >= 100;
            if (!batchReady) {
                continue;
            }

            var symbolGroups = listBatch.GroupBy(x => x.NormalizedSymbol);
            foreach (var group in symbolGroups) {
                var batchOptions = new CreateBatchOptions { PartitionKey = group.Key };
                EventDataBatch eventBatch = await _eventHub.CreateBatchAsync(batchOptions, token);

                foreach (var item in group) {
                    var json = JsonSerializer.Serialize(item);
                    var eventData = new EventData(json) { CorrelationId = item.CorrelationId };
                    eventData.Properties[nameof(item.Symbol)] = item.NormalizedSymbol;
                    eventData.Properties[nameof(item.Timestamp)] = item.Timestamp.ToString("O");

                    if (!eventBatch.TryAdd(eventData)) {
                        try {
                            await _resiliencePipeline.ExecuteAsync(
                                async t => await _eventHub.SendAsync(eventBatch, t),
                                token
                            );
                            Interlocked.Add(ref _publishedCount, eventBatch.Count);
                        } catch (Exception ex) {
                            SendToDeadLetterQueue(item, ex, hostId, ref _droppedCount);
                        }
                        eventBatch = await _eventHub.CreateBatchAsync(batchOptions, token);

                        if (!eventBatch.TryAdd(eventData)) {
                            SendToDeadLetterQueue(item, new InvalidOperationException("Event too large for batch"), hostId, ref _droppedCount);
                        }
                    }
                }

                if (eventBatch.Count > 0) {
                    try {
                        await _resiliencePipeline.ExecuteAsync(
                            async t => await _eventHub.SendAsync(eventBatch, t),
                            token
                        );
                        Interlocked.Add(ref _publishedCount, eventBatch.Count);
                    } catch (Exception ex) {
                        foreach (var item in group) {
                            SendToDeadLetterQueue(item, ex, hostId, ref _droppedCount);
                        }
                    }
                }
            }

            listBatch.Clear();
            batchStartTime = default;
        }

        if (listBatch.Count > 0) {
            var symbolGroups = listBatch.GroupBy(x => x.NormalizedSymbol);
            foreach (var group in symbolGroups) {
                var batchOptions = new CreateBatchOptions { PartitionKey = group.Key };
                EventDataBatch eventBatch = await _eventHub.CreateBatchAsync(batchOptions, token);

                foreach (var item in group) {
                    var json = JsonSerializer.Serialize(item);
                    var eventData = new EventData(json) { CorrelationId = item.CorrelationId };
                    eventData.Properties[nameof(item.Symbol)] = item.NormalizedSymbol;
                    eventData.Properties[nameof(item.Timestamp)] = item.Timestamp.ToString("O");

                    if (!eventBatch.TryAdd(eventData)) {
                        try {
                            await _resiliencePipeline.ExecuteAsync(
                                async t => await _eventHub.SendAsync(eventBatch, t),
                                token
                            );
                            Interlocked.Add(ref _publishedCount, eventBatch.Count);
                        } catch (Exception ex) {
                            SendToDeadLetterQueue(item, ex, hostId, ref _droppedCount);
                        }
                        eventBatch = await _eventHub.CreateBatchAsync(batchOptions, token);
                        if (!eventBatch.TryAdd(eventData)) {
                            SendToDeadLetterQueue(item, new InvalidOperationException("Event too large for batch"), hostId, ref _droppedCount);
                        }
                    }
                }

                if (eventBatch.Count > 0) {
                    try {
                        await _resiliencePipeline.ExecuteAsync(
                            async t => await _eventHub.SendAsync(eventBatch, t),
                            token
                        );
                        Interlocked.Add(ref _publishedCount, eventBatch.Count);
                    } catch (Exception ex) {
                        foreach (var item in group) {
                            SendToDeadLetterQueue(item, ex, hostId, ref _droppedCount);
                        }
                    }
                }
            }
        }

        if (_droppedCount > 0) {
            _logger.LogWarning("Total dropped items in session: {DroppedCount}, HostId: {HostId}", _droppedCount, hostId);
        }
    }

    private async Task<bool> PublishListBatch(List<MarketPrice> listBatch, string hostId, CancellationToken token) {
        try {
            var symbolGroups = listBatch.GroupBy(p => p.NormalizedSymbol);
            foreach (var group in symbolGroups) {
                try {
                    await Publish(group.ToList(), group.Key, token);
                } catch (Exception ex) {
                    _logger.LogError(ex, $"Failed to publish {group.Count()} symbolLiveDataBatch to partition {group.Key}");
                }
            }
            listBatch.Clear();
            return true;
        } catch (Exception ex) {
            foreach (var liveData in listBatch) {
                SendToDeadLetterQueue(liveData, ex, hostId, ref _droppedCount);
            }
            return false;
        }
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

    private void SendToDeadLetterQueue(MarketPrice item, Exception ex, string hostId, ref long droppedCount) {
        _logger.LogError(ex, "Live data liveData dropped. Symbol: {Symbol}, CorrelationId: {CorrelationId}, HostId: {HostId}", item?.Symbol, item?.CorrelationId, hostId);
        Interlocked.Increment(ref droppedCount);
        // Implement your dead-letter logic here (e.g., save to DB, send to queue, etc.)
    }
}