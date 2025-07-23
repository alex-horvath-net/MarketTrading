using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Azure.Messaging.EventHubs.Consumer;
using MarketDataRelayService.Features.RelayLiveMarketData;
using MarketDataRelayService.Domain;


namespace MarketDataRelayService.Infrastructure;
public class EventHubConsumerOptions {
    public string EventHubConnectionString { get; set; } = string.Empty;
    public string EventHubName { get; set; } = string.Empty;
    public string ConsumerGroup { get; set; } = "marketdata-ui";
    public int MaxBatchSize { get; set; } = 100;
    public int PrefetchCount { get; set; } = 300;
    public int ReceiveTimeoutMs { get; set; } = 1000;
}

 
public class EventHubConsumer : IEventHubConsumer {
    private readonly EventHubConsumerOptions _options;
    private readonly ILogger<EventHubConsumer> _logger;

    public EventHubConsumer(IOptions<EventHubConsumerOptions> options, ILogger<EventHubConsumer> logger) {
        _options = options.Value;
        _logger = logger;
    }

    public async IAsyncEnumerable<MarketPrice> ReadAsync([EnumeratorCancellation] CancellationToken token) {
        await using var consumer = new EventHubConsumerClient(
            _options.ConsumerGroup,
            _options.EventHubConnectionString,
            _options.EventHubName);


        await foreach (PartitionEvent partitionEvent in consumer.ReadEventsAsync(token)) {
            MarketPrice? liveData = null;
            try {
                var json = partitionEvent.Data.EventBody.ToString();
                liveData = JsonSerializer.Deserialize<MarketPrice>(json);
            } catch (Exception ex) {
                _logger.LogError(ex, "Invalid message in event hub.");
            }
            if (liveData != null)
                yield return liveData;
        }
    }
}

