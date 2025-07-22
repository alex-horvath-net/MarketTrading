using MarketDataIngestorService.Features.LiveMarketData;

namespace MarketDataIngestorService; 
public class MarketDataIngestorServiceWorker : BackgroundService {
    private readonly ILogger<MarketDataIngestorServiceWorker> _logger;
    private IMarketDataPublisher _marketDataPublisher;

    public MarketDataIngestorServiceWorker(IMarketDataPublisher marketDataPublisher, ILogger<MarketDataIngestorServiceWorker> logger) {
        _marketDataPublisher = marketDataPublisher;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        _logger.LogInformation("MarketDataIngestorServiceWorker started");
        await _marketDataPublisher.RunAsync(stoppingToken);
        _logger.LogInformation("MarketDataIngestorServiceWorker stopped");
    }
}

