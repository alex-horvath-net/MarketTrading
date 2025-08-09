namespace MarketDataIngestionService.Features.IngestLiveMarketData;

public interface IReceiver {
    Task StartReceivingLiveData(string instanceId, CancellationToken token);
}

 