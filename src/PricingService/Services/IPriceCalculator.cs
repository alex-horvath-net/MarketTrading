using PricingService.Models;

namespace PricingService.Services {
    public interface IPriceCalculator {
        Task<OptionPricingResult> CalculatePriceAsync(string symbol, decimal strike, double timeToExpiry);
    }
}
