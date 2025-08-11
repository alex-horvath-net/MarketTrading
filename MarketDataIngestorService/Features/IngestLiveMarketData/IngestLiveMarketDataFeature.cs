using MarketDataIngestionService.Infrastructure.Host;

namespace MarketDataIngestionService.Features.IngestLiveMarketData;

public class IngestLiveMarketDataFeature : IIngestLiveMarketDataFeature {
   
    public async Task RunAsync(string hostId, CancellationToken token) {
        // First, fire-and-forget the publisher as long-running background thread task.
        // Fire-and-forget is safe here because:
        // - Task is captured and awaited below, so exceptions and cancellation are not lost.
        // - Publisher uses the provided CancellationToken for graceful shutdown.
        var publishTask = Task.Run(() => _publisher.PublishLiveData(hostId, token), token);

        // Next, fire-and-forget receiver as a long-running main thread task.
        // Fire-and-forget is safe here because:
        // - Task is captured and awaited below, so exceptions and cancellation are not lost.
        // - Receiver uses the provided CancellationToken for graceful shutdown.
        // - Publisher is already listening for messages, so no data loss occurs.
        var receiveTask = _receiver.StartReceivingLiveData(hostId, token);

        // Await both tasks concurrently. This ensures:
        // - Both publisher and receiver are monitored for errors and cancellation.
        // - The service only completes when both have finished, preventing orphaned background work.
        await Task.WhenAll(publishTask, receiveTask);
    }

    public IngestLiveMarketDataFeature(IReceiver receiver, IPublisher publisher) {
        _receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
        _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
    }

    private readonly IPublisher _publisher;
    private readonly IReceiver _receiver;
}
