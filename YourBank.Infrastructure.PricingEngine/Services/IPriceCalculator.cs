namespace YourBank.Infrastructure.PricingEngine.Services {
    public interface IPriceCalculator {
        Task<decimal> CalculatePriceAsync(string symbol, decimal strike, double timeToExpiry);
    }
}
