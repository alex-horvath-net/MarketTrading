namespace YourBank.Infrastructure.PricingEngine.Services {
    public interface IRiskMetricsService {
        Task<decimal> CalculateVolatilityAsync(string symbol);
    }
}
