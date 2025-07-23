namespace MarketDataRelayService;

public class MarketDataRelayBackgroundService : BackgroundService {
    private readonly IRelayLiveMarketDataFeature _relayLiveMarketDataFeature;
    private readonly ILogger<MarketDataRelayBackgroundService> _logger;

    public MarketDataRelayBackgroundService(IRelayLiveMarketDataFeature relayLiveMarketDataFeature, ILogger<MarketDataRelayBackgroundService> logger) {
        _relayLiveMarketDataFeature = relayLiveMarketDataFeature;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken token) {
        _logger.LogInformation("MarketDataRelayService started");

        await _relayLiveMarketDataFeature.RunAsync(token);

        _logger.LogInformation("MarketDataRelayService stopped");
    }
}

public interface IRelayLiveMarketDataFeature {
    Task RunAsync(CancellationToken token);
}

