using PricingEngineService.Models;

namespace PricingEngineService.Services {
    public interface IPriceCalculator {
        Task<OptionPricingResult> CalculatePriceAsync(string symbol, decimal strike, double timeToExpiry);
    }
}
