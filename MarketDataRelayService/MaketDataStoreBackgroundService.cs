namespace MarketDataRelayService;

public class MarketDataStoreBackgroundService : BackgroundService {
    private readonly ISoreLiveMarketDataFeature _feature;
    private readonly ILogger<MarketDataStoreBackgroundService> _logger;

    public MarketDataStoreBackgroundService(ISoreLiveMarketDataFeature feature, ILogger<MarketDataStoreBackgroundService> logger) {

        _feature = feature ?? throw new ArgumentNullException(nameof(feature));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken token) {
        _logger.LogInformation("MarketDataStoreBackgroundService started.");

        await _feature.RunAsync(token);

        _logger.LogInformation("MarketDataStoreBackgroundService stopped.");
    }
}

public interface ISoreLiveMarketDataFeature { 
    Task RunAsync(CancellationToken token);
}



