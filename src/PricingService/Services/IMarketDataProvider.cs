using PricingService.Models;

namespace PricingService.Services {
    public interface IMarketDataProvider {
        Task<MarketData> GetRealTimeDataAsync(string symbol);
    }
}
