﻿namespace MarketDataRelayService.Features.RelayLiveMarketData;

public interface ISrorageEnqueue {
    void Enqueue(Func<CancellationToken, Task> workItem);
    Task<Func<CancellationToken, Task>> Dequeue(CancellationToken token);
}
