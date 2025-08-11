using MarketDataIngestionService.Features.IngestLiveMarketData;

namespace MarketDataIngestionService.Infrastructure.LiveDataPublisher;
public class LoggingPublisherDecorator : IPublisher {
    private readonly IPublisher _inner;
    private readonly ILogger<LoggingPublisherDecorator> _logger;

    private readonly ITime _time;
    private readonly IMeter _meter;

    public LoggingPublisherDecorator(IPublisher inner, ILogger<LoggingPublisherDecorator> logger, IMeter meter, ITime time) {
        _inner = inner;
        _logger = logger;
        _meter = meter;
        _time = time;
    }

    public async Task PublishLiveData(string hostId, CancellationToken token) {
        var start =_time.UtcNow;
        try {
            _logger.LogInformation("Publishing started.");
           
            await _inner.PublishLiveData(hostId, token);
            
            _meter.Record("publish_success", 1);
        } catch (Exception ex) {
            _logger.LogError(ex, "Publishing failed.");
            _meter.Record("publish_failure", 1);
            throw;
        } finally {
            var duration = _time.UtcNow - start;
            _meter.Record("publish_duration_ms", duration.TotalMilliseconds);
        }
    }
}