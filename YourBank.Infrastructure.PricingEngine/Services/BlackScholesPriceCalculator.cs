using MathNet.Numerics.Distributions;

namespace YourBank.Infrastructure.PricingEngine.Services {
    public class BlackScholesPriceCalculator : IPriceCalculator {
        private readonly IConfiguration _configuration;
        private readonly IMarketDataProvider _marketDataProvider;
        private readonly IRiskMetricsService _riskMetricsService;

        public BlackScholesPriceCalculator(IConfiguration configuration, IMarketDataProvider marketDataProvider, IRiskMetricsService riskMetricsService) {
            _configuration = configuration;
            _marketDataProvider = marketDataProvider;
            _riskMetricsService = riskMetricsService;
        }

        // Calculate price for a European call option using the Black-Scholes model.
        public async Task<decimal> CalculatePriceAsync(string symbol, decimal strike, double timeToExpiry) {
            // Get current market data
            var marketData = await _marketDataProvider.GetRealTimeDataAsync(symbol);
            double S = (double)marketData.LastPrice; // Underlying asset price
            double K = (double)strike;              // Strike price
            double T = timeToExpiry;                // Time to expiry in years
            double r = _configuration.GetValue<double>("Pricing:RiskFreeRate"); // Risk-free rate from config

            // Retrieve volatility from RiskMetrics service (or market data)
            double sigma = (double)(await _riskMetricsService.CalculateVolatilityAsync(symbol));

            // Black-Scholes formula calculations:
            double d1 = (Math.Log(S / K) + (r + 0.5 * sigma * sigma) * T) / (sigma * Math.Sqrt(T));
            double d2 = d1 - sigma * Math.Sqrt(T);

            double N_d1 = Normal.CDF(0, 1, d1);
            double N_d2 = Normal.CDF(0, 1, d2);

            // Call option price
            double callPrice = S * N_d1 - K * Math.Exp(-r * T) * N_d2;
            return (decimal)callPrice;
        }
    }
}
