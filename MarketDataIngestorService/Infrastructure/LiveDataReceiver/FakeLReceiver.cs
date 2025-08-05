using MarketDataIngestionService.Domain;
using MarketDataIngestionService.Features.IngestLiveMarketData;

namespace MarketDataIngestionService.Infrastructure.LiveDataReceiver;

public class FakeLReceiver : IReceiver {
    private readonly IBuffer buffer;
    private readonly Random _random = new Random();
    private readonly ILogger<FakeLReceiver> _logger;

    public FakeLReceiver(IBuffer buffer,  ILogger<FakeLReceiver> logger) {
        this.buffer = buffer;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public async Task StartReceivingLiveData(IEnumerable<string> symbols, string instanceId, CancellationToken token) {

        while (!token.IsCancellationRequested) {
            foreach (var symbol in symbols) {
                ReceiveLiveData( instanceId, symbol);
            }
            await Task.Delay(1000, token);
        }

        buffer.CompleteAdding();
    }

    private void ReceiveLiveData( string instanceId, string symbol) {
        var rawLiveData = ReceiveRawLiveData(symbol);

        if (!ValidateRawLiveData(rawLiveData)) {
            _logger.LogWarning("Invalid live data received. [InstanceId: {InstanceId}]", instanceId);
            return;
        }

        var liveData = MapRawLiveData(rawLiveData);

       buffer.BufferLiveData(liveData, instanceId);
    }

    private dynamic ReceiveRawLiveData(string symbol) => new {
        Symbol = symbol,
        Bid = _random.NextDouble() * 100,
        Ask = _random.NextDouble() * 100,
        Last = _random.NextDouble() * 100,
        Timestamp = DateTime.UtcNow,
        CorrelationId = Guid.NewGuid().ToString()
    };


    private static bool ValidateRawLiveData(dynamic price) =>
        !string.IsNullOrWhiteSpace(price.Symbol) &&
        price.Timestamp != DateTime.UtcNow &&
        price.Bid >= 0 &&
        price.Ask >= 0 &&
        price.Last >= 0 &&
        price.Bid <= 1_000_00 &&
        price.Ask <= 1_000_000 &&
        price.Last <= 1_000_000;

    private static MarketPrice MapRawLiveData(dynamic rawLiveData) => new() {
        Symbol = rawLiveData.Symbol,
        Bid = rawLiveData.Bid,
        Ask = rawLiveData.Ask,
        Last = rawLiveData.Last,
        Timestamp = rawLiveData.Timestamp,
        CorrelationId = rawLiveData.CorrelationId
    };
}

