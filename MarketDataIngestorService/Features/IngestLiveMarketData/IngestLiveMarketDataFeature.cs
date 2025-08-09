using MarketDataIngestionService.Infrastructure.Host;
using System.Diagnostics;

namespace MarketDataIngestionService.Features.IngestLiveMarketData;

public class IngestLiveMarketDataFeature : IIngestLiveMarketDataFeature {
    private readonly IPublisher _publisher;
    private readonly IReceiver _receiver;
    private readonly ILogger<IngestLiveMarketDataFeature> _logger;
    private readonly string _instanceId = Guid.NewGuid().ToString();

    public IngestLiveMarketDataFeature(
        IReceiver receiver,
        IPublisher publisher,
        IRepository repository,
        ILogger<IngestLiveMarketDataFeature> logger) {
        _receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
        _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task RunAsync(CancellationToken token) {
        var stopwatch = Stopwatch.StartNew();
        try {
            _logger.LogInformation("MarketDataIngestionService starting — InstanceId: {InstanceId}", _instanceId);

            var publishTask = Task.Run(() => _publisher.PublishLiveData(token), token);
            var receiveTask = _receiver.StartReceivingLiveData(_instanceId, token);

            await Task.WhenAll(publishTask, receiveTask);
        } catch (OperationCanceledException) {
            _logger.LogWarning("MarketDataIngestionService cancelled — InstanceId: {InstanceId}", _instanceId);
        } catch (Exception ex) {
            _logger.LogError(ex, "MarketDataIngestionService encountered an error — InstanceId: {InstanceId}", _instanceId);
            throw;
        } finally {
            stopwatch.Stop();
            _logger.LogInformation("MarketDataIngestionService stopped — InstanceId: {InstanceId}, Uptime: {ElapsedSeconds}s", _instanceId, stopwatch.Elapsed.TotalSeconds);
        }
    }
}

