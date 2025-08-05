using MarketDataIngestionService.Domain;

namespace MarketDataIngestionService.Features.IngestLiveMarketData;
public interface IBuffer {
    void BufferLiveData(MarketPrice liveData, string instanceId);
    void CompleteAdding();
}
