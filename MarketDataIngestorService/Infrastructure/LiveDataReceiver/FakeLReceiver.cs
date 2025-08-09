using MarketDataIngestionService.Domain;
using MarketDataIngestionService.Features.IngestLiveMarketData;
using System.Diagnostics;

namespace MarketDataIngestionService.Infrastructure.LiveDataReceiver;

public class FakeLReceiver : IReceiver
{
    private readonly IBuffer _buffer;
    private readonly ILogger<FakeLReceiver> _logger;
    private readonly Random _random;
    private readonly ITime _time;
    private readonly IRepository _repository;

    public FakeLReceiver(IBuffer buffer, ILogger<FakeLReceiver> logger, ITime time, IRepository repository)
    {
        _buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _time = time ?? throw new ArgumentNullException(nameof(time));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _random = new Random();
    }

    public async Task StartReceivingLiveData(string instanceId, CancellationToken token)
    {
        int receivedCount = 0, failedCount = 0, invalidCount = 0;
        var stopwatch = Stopwatch.StartNew();

        IEnumerable<string> symbols;
        try
        {
            // All retry logic is handled by the repository itself.
            symbols = await _repository.LoadSymbols(token);
            ValidateArguments(symbols, instanceId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading symbols [InstanceId: {InstanceId}]", instanceId);
            throw;
        }

        var lastHealthLog = _time.UtcNow;
        try
        {
            while (!token.IsCancellationRequested)
            {
                foreach (var symbol in symbols)
                {
                    try
                    {
                        if (TryAddMarketPrice(symbol, instanceId))
                        {
                            receivedCount++;
                        }
                        else
                        {
                            invalidCount++;
                        } 
                    }
                    catch (Exception ex)
                    {
                        failedCount++;
                        _logger.LogError(ex, "Error adding market price [InstanceId: {InstanceId}, Symbol: {Symbol}]", instanceId, symbol);
                    }
                }
                await Task.Delay(1000, token);

                // Health/metrics log every 10 seconds
                if ((_time.UtcNow - lastHealthLog).TotalSeconds >= 10)
                {
                    _logger.LogInformation(
                        "Health: [InstanceId: {InstanceId}] Received: {ReceivedCount}, Invalid: {InvalidCount}, Failed: {FailedCount}, Uptime: {ElapsedSeconds}s",
                        instanceId, receivedCount, invalidCount, failedCount, stopwatch.Elapsed.TotalSeconds);
                    lastHealthLog = _time.UtcNow;
                }
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Receiving cancelled [InstanceId: {InstanceId}]", instanceId);
        }
        finally
        {
            _buffer.StopAddItem();
            stopwatch.Stop();
            _logger.LogInformation(
                "Stopped receiving live data [InstanceId: {InstanceId}], TotalReceived: {ReceivedCount}, Invalid: {InvalidCount}, Failed: {FailedCount}, Uptime: {ElapsedSeconds}s",
                instanceId, receivedCount, invalidCount, failedCount, stopwatch.Elapsed.TotalSeconds);
        }
    }

    private bool TryAddMarketPrice(string symbol, string instanceId)
    {
        if (string.IsNullOrWhiteSpace(symbol))
        {
            _logger.LogWarning("Symbol is null or whitespace. [InstanceId: {InstanceId}]", instanceId);
            return false;
        }

        var marketPrice = CreateMarketPrice(symbol);
        if (IsValidMarketPrice(marketPrice))
        {
            _buffer.AddItem(marketPrice, instanceId);
            return true;
        }
        else
        {
            _logger.LogWarning("Invalid live data received. [InstanceId: {InstanceId}, Symbol: {Symbol}]", instanceId, symbol);
            return false;
        }
    }

    private MarketPrice CreateMarketPrice(string symbol) => new MarketPrice
    {
        Symbol = symbol,
        Bid = Math.Round(_random.NextDouble() * 100, 4),
        Ask = Math.Round(_random.NextDouble() * 100, 4),
        Last = Math.Round(_random.NextDouble() * 100, 4),
        Timestamp = _time.UtcNow,
        CorrelationId = Guid.NewGuid().ToString()
    };

    private static bool IsValidMarketPrice(MarketPrice price) =>
        price is not null &&
        !string.IsNullOrWhiteSpace(price.Symbol) &&
        price.Bid is >= 0 and <= 100_000 &&
        price.Ask is >= 0 and <= 1_000_000 &&
        price.Last is >= 0 and <= 1_000_000;

    private static void ValidateArguments(IEnumerable<string> symbols, string instanceId)
    {
        if (symbols == null) throw new ArgumentNullException(nameof(symbols));
        if (string.IsNullOrWhiteSpace(instanceId)) throw new ArgumentException("InstanceId cannot be null or whitespace.", nameof(instanceId));
    }
}

