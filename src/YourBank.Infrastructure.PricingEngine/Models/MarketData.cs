namespace PricingEngineService.Models {
    public class MarketData {
        public string Symbol { get; set; } = string.Empty;
        public decimal LastPrice { get; set; }
        public decimal Volatility { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
