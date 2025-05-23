using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace ApiGateway.Client.Trader;

public class TraderServiceClient : ITraderServiceClient {
    private readonly HttpClient traderService;
    private readonly ILogger<TraderServiceClient> logger;

    public TraderServiceClient(HttpClient http, ILogger<TraderServiceClient> logger) {
        this.traderService = http;
        this.logger = logger;
    }

    public async Task<Guid> PlaceTradeAsync(PlaceTradeRequest request, CancellationToken cancellationToken = default) {
        var response = await traderService.PostAsJsonAsync("/api/trades", request, cancellationToken);

        if (!response.IsSuccessStatusCode) {
            var error = await response.Content.ReadAsStringAsync();
            logger.LogError("Failed to place trade. StatusCode: {Code}, Error: {Error}", response.StatusCode, error);
            throw new InvalidOperationException($"PlaceTrade failed: {response.StatusCode}");
        }

        var tradeId = await response.Content.ReadFromJsonAsync<Guid>(cancellationToken);
        if (tradeId == Guid.Empty)
            throw new InvalidOperationException("Empty Trade ID returned");
        return tradeId;
    }

    public async Task<TradeDto?> GetTradeByIdAsync(Guid tradeId, CancellationToken cancellationToken = default) {
        return await traderService.GetFromJsonAsync<TradeDto>($"/api/trades/{tradeId}", cancellationToken);
    }

    public async Task<IEnumerable<TradeDto>> GetRecentTradesAsync(CancellationToken cancellationToken = default) {
        return await traderService.GetFromJsonAsync<List<TradeDto>>("/api/trades", cancellationToken)
               ?? Enumerable.Empty<TradeDto>();
    }
}
