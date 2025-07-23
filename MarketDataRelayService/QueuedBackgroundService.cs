using System.Runtime.CompilerServices;
using MarketDataRelayService.Features.RelayLiveMarketData;

namespace MarketDataRelayService;

public class QueuedBackgroundService : BackgroundService {
    private readonly IQueue _queue;
    private readonly ILogger<QueuedBackgroundService> _logger;

    public QueuedBackgroundService(IQueue queue, ILogger<QueuedBackgroundService> logger) {
        _queue = queue;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken token) {
        _logger.LogInformation("QueuedBackgroundService started.");

        await foreach (var workItem in DequeueLoop(token)) {
            try {
                await workItem(token);
            } catch (Exception ex) {
                _logger.LogError(ex, "⚠️ Error executing background task.");
            }
        }

        _logger.LogInformation("QueuedBackgroundService stopped.");
    }

    private async IAsyncEnumerable<Func<CancellationToken, Task>> DequeueLoop([EnumeratorCancellation] CancellationToken token) {
        while (!token.IsCancellationRequested) {
            yield return await _queue.Dequeue(token);
        }
    }
}

public interface IQueuedBackgroundService {
    Task ExecuteAsync(CancellationToken token);
}



