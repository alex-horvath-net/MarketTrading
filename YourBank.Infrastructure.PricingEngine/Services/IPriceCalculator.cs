using YourBank.Infrastructure.PricingEngine.Models;

namespace YourBank.Infrastructure.PricingEngine.Services {
    public interface IPriceCalculator {
        Task<OptionPricingResult> CalculatePriceAsync(string symbol, decimal strike, double timeToExpiry);
    }
}
