using Infrastructure.Adapters.Blazor;

namespace ApiGateway.Client.Trader.FindTrades; 
public record FindTradesInputModel(string TraderId) : InputModel(TraderId) {
    public string? Instrument { get; set; }
    public string? Side { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
