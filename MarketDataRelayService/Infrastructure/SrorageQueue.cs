using System.Collections.Concurrent;
using MarketDataRelayService.Features.RelayLiveMarketData;
using MarketDataRelayService.Features.SoreLiveMarketData;

namespace MarketDataRelayService.Infrastructure;

public class SrorageQueue : ISrorageEnqueue, ISrorageDequeue {
    private readonly ConcurrentQueue<Func<CancellationToken, Task>> _workItems = new();
    private readonly SemaphoreSlim _signal = new(0);

    public void Enqueue(Func<CancellationToken, Task> workItem) {
        ArgumentNullException.ThrowIfNull(workItem);

        _workItems.Enqueue(workItem);
        _signal.Release();
    }

    public async Task<Func<CancellationToken, Task>> Dequeue(CancellationToken cancellationToken) {
        await _signal.WaitAsync(cancellationToken);

        _workItems.TryDequeue(out var workItem);
        return workItem!;
    }
}
