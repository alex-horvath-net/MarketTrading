namespace MarketDataIngestorService.Features.AcquiereLiveMarketData;
public interface IRepository {
    Task<IEnumerable<string>> LoadSymbols(CancellationToken token);
}
