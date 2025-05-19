namespace PricingService.Services {
    public interface IRiskMetricsService {
        Task<decimal> CalculateVolatilityAsync(string symbol);
    }
}
