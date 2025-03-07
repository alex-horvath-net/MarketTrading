using MathNet.Numerics.Distributions;
using YourBank.Infrastructure.PricingEngine.Models;

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

        public async Task<OptionPricingResult> CalculatePriceAsync(string symbol, decimal strike, double timeToExpiry) {
            // 1. Retrieve market data
            var marketData = await _marketDataProvider.GetRealTimeDataAsync(symbol);
            double S = (double)marketData.LastPrice; // Underlying asset price
            double K = (double)strike;              // Strike price
            double T = timeToExpiry;                // Time to expiry (in years)
            double r = _configuration.GetValue<double>("Pricing:RiskFreeRate"); // Risk-free rate
            double sigma = (double)(await _riskMetricsService.CalculateVolatilityAsync(symbol)); // Volatility

            // 2. Compute d1 and d2 for Black-Scholes
            double d1 = (Math.Log(S / K) + (r + 0.5 * sigma * sigma) * T) / (sigma * Math.Sqrt(T));
            double d2 = d1 - sigma * Math.Sqrt(T);

            // 3. Primary Greeks using standard normal CDF and PDF:
            double Nd1 = Normal.CDF(0, 1, d1);      // CDF at d1
            double Nd2 = Normal.CDF(0, 1, d2);      // CDF at d2
            double pdf_d1 = Normal.PDF(0, 1, d1);     // PDF at d1

            // 4. Calculate the theoretical call option price
            double callPrice = S * Nd1 - K * Math.Exp(-r * T) * Nd2;

            // 5. Primary Greeks:
            double delta = Nd1; // Delta: sensitivity to underlying price.
            double gamma = pdf_d1 / (S * sigma * Math.Sqrt(T)); // Gamma: curvature of price vs. underlying.
            double vega = S * Math.Sqrt(T) * pdf_d1; // Vega: sensitivity to volatility (per 1 percentage point change).
            double theta = -(S * sigma * pdf_d1) / (2 * Math.Sqrt(T)) - r * K * Math.Exp(-r * T) * Nd2; // Theta: time decay.
            double rho = K * T * Math.Exp(-r * T) * Nd2; // Rho: sensitivity to interest rate.

            // 6. Secondary Greeks:
            // Charm: Rate of change of delta with respect to time.
            double charm = -(pdf_d1 / (2 * sigma * T)) * (2 * r * T - sigma * Math.Sqrt(T) * d2);

            // Vanna: Sensitivity of delta to changes in volatility.
            // One common formula: Vanna = -sqrt(T)*pdf(d1)*d1/sigma
            double vanna = -Math.Sqrt(T) * pdf_d1 * d1 / sigma;

            // Vomma (or Volga): Sensitivity of vega to changes in volatility.
            // Vomma = Vega * (d1*d2 - 1) / sigma
            double vomma = vega * (d1 * d2 - 1) / sigma;

            // Speed: Derivative of gamma with respect to underlying price.
            // Speed = -gamma/S * (1 + d1/(sigma*Math.Sqrt(T)))
            double speed = -gamma / S * (1 + d1 / (sigma * Math.Sqrt(T)));

            // Color: Time decay of gamma (Gamma decay).
            // Color = -pdf(d1) * (d1 * sigma * Math.Sqrt(T) + 3) / (2 * S * sigma * Math.Pow(T, 1.5))
            double color = -pdf_d1 * (d1 * sigma * Math.Sqrt(T) + 3) / (2 * S * sigma * Math.Pow(T, 1.5));

            // Zomma: Sensitivity of gamma to changes in volatility.
            // Zomma = Gamma * (d1*d2 - 1) / sigma
            double zomma = gamma * (d1 * d2 - 1) / sigma;

            // Lambda (Elasticity): Percentage change in option price for a percentage change in underlying price.
            double lambda = (S * delta) / callPrice;

            // 7. Package and return all results.
            return new OptionPricingResult {
                Price = (decimal)callPrice,
                Delta = delta, // Measures how much the option price changes with a small change in the underlying price.
                Gamma = gamma,
                Vega = vega,
                Theta = theta,
                Rho = rho,
                Charm = charm,
                Vanna = vanna,
                Vomma = vomma,
                Speed = speed,
                Color = color,
                Zomma = zomma,
                Lambda = lambda
            };
        }
    }
}
