namespace MarketDataIngestionService.Features.IngestLiveMarketData;
public interface IRepository {
    Task<IEnumerable<string>> LoadSymbols(CancellationToken token);
}
