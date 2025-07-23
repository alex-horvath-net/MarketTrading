namespace MarketDataRelayService.Features.SoreLiveMarketData;

public interface ISrorageDequeue {
    void Enqueue(Func<CancellationToken, Task> workItem);
    Task<Func<CancellationToken, Task>> Dequeue(CancellationToken token);
}