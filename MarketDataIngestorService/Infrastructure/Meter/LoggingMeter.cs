namespace MarketDataIngestionService.Infrastructure.Meter;

using MarketDataIngestionService.Features.IngestLiveMarketData;
using Microsoft.Extensions.Logging;

public class LoggingMeter : IMeter {

    public void Record(string metricName, double value) {
        _logger.LogInformation("Metric recorded: {MetricName} = {Value}", metricName, value);
    }

    public LoggingMeter(ILogger<LoggingMeter> logger) {
        _logger = logger;
    }

    private readonly ILogger<LoggingMeter> _logger;
}