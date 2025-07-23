using System.Runtime.CompilerServices;

namespace MarketDataRelayService.Features.SoreLiveMarketData;
public class SoreLiveMarketDataFeature : ISoreLiveMarketDataFeature {
    private ISrorageDequeue _queue;
    private ILogger<SoreLiveMarketDataFeature> _logger;

    public SoreLiveMarketDataFeature(ISrorageDequeue queue, ILogger<SoreLiveMarketDataFeature> logger) {

        _queue = queue ?? throw new ArgumentNullException(nameof(queue));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public async Task RunAsync(CancellationToken token) {
        _logger.LogInformation("MarketDataStoreBackgroundService started.");

        await foreach (var workItem in DequeueLoop(token)) {
            try {
                await workItem(token);
            } catch (Exception ex) {
                _logger.LogError(ex, "⚠️ Error executing background task.");
            }
        }

        _logger.LogInformation("MarketDataStoreBackgroundService stopped.");
    }


    private async IAsyncEnumerable<Func<CancellationToken, Task>> DequeueLoop([EnumeratorCancellation] CancellationToken token) {
        while (!token.IsCancellationRequested) {
            yield return await _queue.Dequeue(token);
        }
    }
}
