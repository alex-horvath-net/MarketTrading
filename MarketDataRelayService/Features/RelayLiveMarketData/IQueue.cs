namespace MarketDataRelayService.Features.RelayLiveMarketData;

public interface IQueue {
    void Enqueue(Func<CancellationToken, Task> workItem);
    Task<Func<CancellationToken, Task>> Dequeue(CancellationToken token);
}
