using YourBank.Infrastructure.PricingEngine.Models;

namespace YourBank.Infrastructure.PricingEngine.Services {
    public interface IMarketDataProvider {
        Task<MarketData> GetRealTimeDataAsync(string symbol);
    }
}
