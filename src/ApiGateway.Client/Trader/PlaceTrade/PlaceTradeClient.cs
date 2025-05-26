using System.Net.Http.Json;
using Infrastructure.Adapters.Blazor;

namespace ApiGateway.Client.Trader.PlaceTrade;

public class PlaceTradeClient : IPlaceTradeClient {
    private readonly HttpClient _http;

    public PlaceTradeInputModel InputModel { get; set; }
    public PlaceTradeViewModel ViewModel { get; } = new();

    public PlaceTradeClient(HttpClient http) {
        _http = http;
        InputModel = new PlaceTradeInputModel("");
    }

    public async Task Execute(CancellationToken cancellationToken = default) {
        ViewModel.Errors.Clear();
        ViewModel.Result = string.Empty;

        try {
            var response = await _http.PostAsJsonAsync("api/trades", InputModel, cancellationToken);
            if (response.IsSuccessStatusCode) {
                var tradeId = await response.Content.ReadFromJsonAsync<Guid>(cancellationToken: cancellationToken);
                ViewModel.Result = $"✅ Trade submitted: {tradeId}";
                ViewModel.AlertCssClass = "alert-success";
            } else {
                var errorText = await response.Content.ReadAsStringAsync(cancellationToken);
                ViewModel.Errors.Add($"❌ Error: {response.StatusCode} - {errorText}");
                ViewModel.AlertCssClass = "alert-danger";
            }
        } catch (Exception ex) {
            ViewModel.Errors.Add($"❌ Exception: {ex.Message}");
            ViewModel.AlertCssClass = "alert-danger";
        }
    }

    private PlaceTradeRequest InputModelToRequest() => new(
       Id: InputModel.Id ?? Guid.NewGuid(),
       Issuer: InputModel.Issuer,
       TraderId: InputModel.TraderId,
       Instrument: InputModel.Instrument,
       Side: InputModel.Side,
       Quantity: InputModel.Quantity,
       Price: InputModel.Price,
       OrderType: InputModel.OrderType,
       TimeInForce: InputModel.TimeInForce,
       StrategyCode: InputModel.StrategyCode,
       PortfolioCode: InputModel.PortfolioCode,
       UserComment: InputModel.UserComment,
       ExecutionRequestedForUtc: InputModel.ExecutionRequestedFor
   );

    private PlaceTradeViewModel FromResponse(PlaceTradeResponse response) {
        var viewModel = new PlaceTradeViewModel();
        viewModel.Meta = ToMetaVM(response.Request);
        viewModel.Errors = response.Errors.Select(ToErrorVM).ToList();
        viewModel.Trade = ToTradeVM(response.Trade);
        viewModel.Result = $"✅ Trade submitted: {response.Trade.Id}";
        viewModel.AlertCssClass = "alert-success";

        return viewModel;

        MetaVM ToMetaVM(PlaceTradeRequest x) =>
            new() { Id = x.Id };

        TradeVM ToTradeVM(Trade x) =>
            new() { Id = x.TraderId, Name = x.Instrument };

        ErrorVM ToErrorVM(Error error) =>
            new() { Name = error.Name, Message = error.Message };
    }

    private PlaceTradeViewModel FromException(Exception ex) {
        var viewModel = new PlaceTradeViewModel();
        viewModel.Errors = [ToErrorVM(ex)];
        viewModel.Result = $"❌ {ex.Message}";
        viewModel.AlertCssClass = "alert-danger";

        return viewModel;

        ErrorVM ToErrorVM(Exception exception) =>
            new() { Name = "", Message = exception.Message };

    }
}

internal class PlaceTradeClient2(HttpClient http) : IPlaceTradeClient {
    public async Task<Guid> PlaceTrade2(PlaceTradeRequest request, CancellationToken cancellationToken = default) {
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

    public PlaceTradeInputModel InputModel { get; set; }
    public PlaceTradeViewModel ViewModel { get; set; }
    // Blazor UI should be abel to call this adapter with no effort or leaking any UI technology 
    public async Task Execute(CancellationToken token) {

        var request = ToRequest();
        var response = await feature.Execute(request, token);
        ViewModel =
            response.Exception == null ?
            FromResponse(response) :
            FromException(response.Exception);
    }

    private PlaceTradeRequest ToRequest() => new(
        Id: InputModel.Id ?? Guid.NewGuid(),
        Issuer: InputModel.Issuer,
        TraderId: InputModel.TraderId,
        Instrument: InputModel.Instrument,
        Side: InputModel.Side,
        Quantity: InputModel.Quantity,
        Price: InputModel.Price,
        OrderType: InputModel.OrderType,
        TimeInForce: InputModel.TimeInForce,
        StrategyCode: InputModel.StrategyCode,
        PortfolioCode: InputModel.PortfolioCode,
        UserComment: InputModel.UserComment,
        ExecutionRequestedForUtc: InputModel.ExecutionRequestedFor
    );

    private PlaceTradeViewModel FromResponse(PlaceTradeResponse response) {
        var viewModel = new PlaceTradeViewModel();
        viewModel.Meta = ToMetaVM(response.Request);
        viewModel.Errors = response.Errors.Select(ToErrorVM).ToList();
        viewModel.Trade = ToTradeVM(response.Trade);
        viewModel.Result = $"✅ Trade submitted: {response.Trade.Id}";
        viewModel.AlertCssClass = "alert-success";

        return viewModel;

        MetaVM ToMetaVM(PlaceTradeRequest x) =>
            new() { Id = x.Id };

        TradeVM ToTradeVM(Trade x) =>
            new() { Id = x.TraderId, Name = x.Instrument };

        ErrorVM ToErrorVM(Error error) =>
            new() { Name = error.Name, Message = error.Message };
    }

    private PlaceTradeViewModel FromException(Exception ex) {
        var viewModel = new PlaceTradeViewModel();
        viewModel.Errors = [ToErrorVM(ex)];
        viewModel.Result = $"❌ {ex.Message}";
        viewModel.AlertCssClass = "alert-danger";

        return viewModel;

        ErrorVM ToErrorVM(Exception exception) =>
            new() { Name = "", Message = exception.Message };

    }
}
