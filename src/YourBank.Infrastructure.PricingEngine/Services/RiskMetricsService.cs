namespace YourBank.Infrastructure.PricingEngine.Services {
    public class RiskMetricsService : IRiskMetricsService {
        private readonly IMarketDataProvider _marketDataProvider;

        public RiskMetricsService(IMarketDataProvider marketDataProvider) {
            _marketDataProvider = marketDataProvider;
        }

        public async Task<decimal> CalculateVolatilityAsync(string symbol) {
            // In a real system, fetch historical prices and compute standard deviation.
            // For this demo, we simply return the volatility value from the real-time data.
            var marketData = await _marketDataProvider.GetRealTimeDataAsync(symbol);
            return marketData.Volatility;
        }
    }
}
