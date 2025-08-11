using MarketDataIngestionService.Domain;
using MarketDataIngestionService.Infrastructure.Buffer;

namespace MarketDataIngestionService.Features.IngestLiveMarketData;
public interface IBuffer {
    BufferOptions Options { get; }

    void AddItem(MarketPrice liveData, string hostId);
    void StopAddItem();
    IAsyncEnumerable<MarketPrice> GetItemsAsync(CancellationToken cancellationToken);
}
