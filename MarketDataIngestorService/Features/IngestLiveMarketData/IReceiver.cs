namespace MarketDataIngestionService.Features.IngestLiveMarketData;

public interface IReceiver {
    Task StartReceivingLiveData(string hostId, CancellationToken token);
}

 