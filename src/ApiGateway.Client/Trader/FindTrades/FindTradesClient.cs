using Infrastructure.Adapters.Blazor;
using TradingService.FindTrades;

namespace ApiGateway.Client.Trader.FindTrades;
public class FindTradesClient(HttpClient http) : IFindTradesClient {

    public FindTradesInputModel InputModel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public FindTradesViewModel ViewModel => throw new NotImplementedException();

    public Task Execute(CancellationToken cancellationToken = default) {
        throw new NotImplementedException();
    }
}



////
//public record FindTradesInputModel(string TraderId) : InputModel(TraderId) {
//    public string? Instrument { get; set; }
//    public string? Side { get; set; }
//    public DateTime? FromDate { get; set; }
//    public DateTime? ToDate { get; set; }
//}
//public record FindTradesViewModel : ViewModel {
//    public TableModel<Trade> Trades { get; set; } = new();
//    public int TotalCount { get; set; }
//    public int BuyCount { get; set; }
//    public int SellCount { get; set; }
//}

//public interface IFeatureAdapter {
//    FindTradesInputModel InputModel { get; set; }
//    FindTradesViewModel ViewModel { get; set; }
//    Task Execute(CancellationToken token);
//}

//internal class FeatureAdapter(IFeature feature) : IFeatureAdapter {
//    public FindTradesInputModel InputModel { get; set; }
//    public FindTradesViewModel ViewModel { get; set; }
//    // Blazor should be abel to call this adapter with minimum effort and zero technology leaking
//    public async Task Execute(CancellationToken token) {
//        var request = ToRequest(InputModel);
//        var response = await feature.Execute(request, token);
//        ViewModel = ToViewModel(response);
//    }

//    public FindTradesRequest ToRequest(FindTradesInputModel inputModel) => new(
//        Id: Guid.NewGuid(),
//        inputModel.Issuer,
//        inputModel.TraderId,
//        inputModel.Instrument,
//        inputModel.Side,
//        inputModel.FromDate,
//        inputModel.ToDate);

//    internal static FindTradesViewModel ToViewModel(FindTradesResponse response) => new FindTradesViewModel() {
//        Meta = new MetaVM() {
//            Id = response.Request.Id
//        },
//        Errors = response.Errors.Select(x => new ErrorVM() {
//            Name = x.Name,
//            Message = x.Message
//        }),
//        Trades = new TableModel<Trade>() {
//            Rows = response.Trades,
//            Columns = [
//                trade => trade.Instrument,
//                trade => trade.Side,
//                trade => trade.SubmittedAt,
//                trade => trade.Status
//            ]
//        },
//        TotalCount = response.Trades.Count(),
//        BuyCount = response.Trades.Count(trade => trade.Side == TradeSide.Buy),
//        SellCount = response.Trades.Count(trade => trade.Side == TradeSide.Sell),
//    };
//}


