namespace MarketDataIngestionService.Infrastructure.Host;

public interface IIngestLiveMarketDataFeature {
    Task RunAsync(string _instanceId, CancellationToken token);
}

