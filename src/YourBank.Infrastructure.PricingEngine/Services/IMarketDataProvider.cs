using PricingEngineService.Models;

namespace PricingEngineService.Services {
    public interface IMarketDataProvider {
        Task<MarketData> GetRealTimeDataAsync(string symbol);
    }
}
