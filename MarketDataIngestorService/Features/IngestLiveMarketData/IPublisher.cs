using MarketDataIngestionService.Domain;

namespace MarketDataIngestionService.Features.LiveMarketData {
    public interface IPublisher {
        Task Publish(IEnumerable<MarketPrice> liveDataBatch, string symbol, CancellationToken token);
    }
}