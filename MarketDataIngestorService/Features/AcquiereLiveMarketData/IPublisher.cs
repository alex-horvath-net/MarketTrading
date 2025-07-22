// MarketDataPublisher.cs — Uses IEventHubClient abstraction only
using MarketDataIngestorService.Domain;

namespace MarketDataIngestorService.Features.LiveMarketData {
    public interface IPublisher {
        Task SendAsync(MarketPrice price, CancellationToken cancellationToken);
    }
}