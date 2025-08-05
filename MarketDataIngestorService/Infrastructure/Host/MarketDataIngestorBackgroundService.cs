namespace MarketDataIngestionService.Infrastructure.Host; 
public class MarketDataIngestorBackgroundService : BackgroundService {
    private readonly ILogger<MarketDataIngestorBackgroundService> _logger;
    private IIngestLiveMarketDataFeature _ingestLiveMarketData;

    public MarketDataIngestorBackgroundService(IIngestLiveMarketDataFeature acquiereLiveMarketData, ILogger<MarketDataIngestorBackgroundService> logger) {
        _ingestLiveMarketData = acquiereLiveMarketData;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken token) {
        _logger.LogInformation("MarketDataIngestorBackgroundService started");
        await _ingestLiveMarketData.RunAsync(token);
        _logger.LogInformation("MarketDataIngestorBackgroundService stopped");
    }
}
