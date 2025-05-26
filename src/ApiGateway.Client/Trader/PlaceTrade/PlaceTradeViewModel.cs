using System.ComponentModel;
using Infrastructure.Adapters.Blazor;

namespace ApiGateway.Client.Trader.PlaceTrade;

public record PlaceTradeViewModel : ViewModel {
    public string? Result { get; set; }
    public string AlertCssClass { get; set; } = "alert-info";

    public TradeVM Trade { get; set; }

    public record TradeVM {
        [DisplayName("ID")]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
