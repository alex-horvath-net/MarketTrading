namespace MarketDataIngestionService.Features.IngestLiveMarketData;
public interface IMeter

{
    void Record(string metricName, double value);
}