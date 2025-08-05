namespace MarketDataIngestionService.Infrastructure.Host;

public interface IIngestLiveMarketDataFeature {
    Task RunAsync(CancellationToken token);
}

