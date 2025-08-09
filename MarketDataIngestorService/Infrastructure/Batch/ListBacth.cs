using MarketDataIngestionService.Domain;
using MarketDataIngestionService.Features.IngestLiveMarketData;
using Microsoft.Extensions.Options;

namespace MarketDataIngestionService.Infrastructure.Batch;
public class ListBacth : IListBacth {

    private readonly ITime _time;
    private readonly BatchOptions _options;
    private List<MarketPrice> listBatch;
    private DateTime lastPublish;

    public ListBacth(ITime time, IOptions<BatchOptions> options) {
        _time = time;
        _options = options.Value;

        lastPublish = _time.UtcNow;
        listBatch = new List<MarketPrice>(_options.Size);
    }

    public int Count => listBatch.Count;

    public bool Add(MarketPrice liveData, ref List<MarketPrice> batch) {
        listBatch.Add(liveData);

        bool sizeTrigger = listBatch.Count >= _options.Size;
        bool timeTrigger = (_time.UtcNow - lastPublish).TotalMilliseconds >= _options.IntervalMs;
        if (sizeTrigger || timeTrigger) {
            batch = new List<MarketPrice>(listBatch);
            listBatch.Clear();
            lastPublish = _time.UtcNow;
            return true;
        }
        return false;
    }
}
