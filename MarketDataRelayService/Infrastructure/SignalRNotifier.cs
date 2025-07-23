using MarketDataRelayService.Domain;
using MarketDataRelayService.Features.RelayLiveMarketData;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;

namespace MarketDataRelayService.Infrastructure;
public class SignalRNotifierOptions {
    public string HubUrl { get; set; } = "https://your-ui-host/signalr/marketdata";
    public string MethodName { get; set; } = "ReceiveMarketPrice";
}

public class SignalRNotifier : IClientNotifier {
    private readonly ILogger<SignalRNotifier> _logger;
    private readonly HubConnection _connection;
    private readonly SignalRNotifierOptions _options;

    public SignalRNotifier(IOptions<SignalRNotifierOptions> options, ILogger<SignalRNotifier> logger) {
        _options = options.Value;
        _logger = logger;

        _connection = new HubConnectionBuilder()
            .WithUrl(_options.HubUrl) 
            .WithAutomaticReconnect()
            .Build();
    }

    public async Task SendAsync(MarketPrice liveData, CancellationToken token) {
        try {
            if (_connection.State != HubConnectionState.Connected) {
                _logger.LogInformation("Connecting to SignalR hub...");
                await _connection.StartAsync(token);
            }

            await _connection.InvokeAsync(_options.MethodName, liveData, token);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to send market liveData to SignalR hub");
        }
    }
}
