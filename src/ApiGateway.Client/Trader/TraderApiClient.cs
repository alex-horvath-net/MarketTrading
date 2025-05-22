using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace ApiGateway.Client.Trader;

public class TraderApiClient : ITraderApiClient {
    private readonly HttpClient _http;
    private readonly ILogger<TraderApiClient> _logger;

    public TraderApiClient(HttpClient http, ILogger<TraderApiClient> logger) {
        _http = http;
        _logger = logger;
    }

    public async Task<Guid> PlaceTradeAsync(PlaceTradeRequest request, CancellationToken cancellationToken = default) {
        var response = await _http.PostAsJsonAsync("/api/trades", request, cancellationToken);

        if (!response.IsSuccessStatusCode) {
            var error = await response.Content.ReadAsStringAsync();
            _logger.LogError("Failed to place trade. StatusCode: {Code}, Error: {Error}", response.StatusCode, error);
            throw new InvalidOperationException($"PlaceTrade failed: {response.StatusCode}");
        }

        var tradeId = await response.Content.ReadFromJsonAsync<Guid>(cancellationToken);
        if (tradeId == Guid.Empty)
            throw new InvalidOperationException("Empty Trade ID returned");
        return tradeId;
    }

    public async Task<TradeDto?> GetTradeByIdAsync(Guid tradeId, CancellationToken cancellationToken = default) {
        return await _http.GetFromJsonAsync<TradeDto>($"/api/trades/{tradeId}", cancellationToken);
    }

    public async Task<IEnumerable<TradeDto>> GetRecentTradesAsync(CancellationToken cancellationToken = default) {
        return await _http.GetFromJsonAsync<List<TradeDto>>("/api/trades", cancellationToken)
               ?? Enumerable.Empty<TradeDto>();
    }
}
