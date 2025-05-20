namespace ApiGateway.Client;

public interface ITraderApiClient {
    Task<Guid> PlaceTradeAsync(PlaceTradeRequest request, CancellationToken cancellationToken = default);
    Task<TradeDto?> GetTradeByIdAsync(Guid tradeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TradeDto>> GetRecentTradesAsync(CancellationToken cancellationToken = default);
}
