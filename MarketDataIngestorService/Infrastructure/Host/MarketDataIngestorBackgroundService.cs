using System.Diagnostics;

namespace MarketDataIngestionService.Infrastructure.Host; 
public class MarketDataIngestorBackgroundService : BackgroundService {
    public MarketDataIngestorBackgroundService(IIngestLiveMarketDataFeature feature, ILogger<MarketDataIngestorBackgroundService> logger) {
        _feature = feature;
        _logger = logger;
        _hostId = Guid.NewGuid().ToString();
    }

    protected override async Task ExecuteAsync(CancellationToken token) {
        var stopwatch = Stopwatch.StartNew();
        try {
            _logger.LogInformation("MarketDataIngestorBackgroundService started — HostId: {HostId}", _hostId);
            await _feature.RunAsync(_hostId, token);
        } catch (OperationCanceledException) {
            _logger.LogWarning("MarketDataIngestorBackgroundService cancelled — HostId: {HostId}", _hostId);
        } catch (Exception ex) {
            _logger.LogError(ex, "MarketDataIngestorBackgroundService failed — HostId: {HostId}", _hostId);
            throw;
        } finally {
            stopwatch.Stop();
            _logger.LogInformation("MarketDataIngestorBackgroundService stopped — HostId: {HostId}, Uptime: {ElapsedSeconds}s", _hostId, stopwatch.Elapsed.TotalSeconds);
        }
    }

    private readonly IIngestLiveMarketDataFeature _feature;
    private readonly ILogger<MarketDataIngestorBackgroundService> _logger;
    private readonly string _hostId;
}
