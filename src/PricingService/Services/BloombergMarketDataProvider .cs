using PricingService.Models;

namespace PricingService.Services {
    public class BloombergMarketDataProvider : IMarketDataProvider {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public BloombergMarketDataProvider(IConfiguration configuration, HttpClient httpClient) {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<MarketData> GetRealTimeDataAsync(string symbol) {
            // Simulate a call to Bloomberg API.
            // In a real implementation, use _httpClient to call the API endpoint with the API key.
            await Task.Delay(100); // simulate network latency

            // Return dummy data for demonstration.
            return new MarketData {
                Symbol = symbol,
                LastPrice = 100 + new Random().Next(0, 50), // random price between 100 and 150
                Volatility = 0.20m, // example volatility (20%)
                Timestamp = DateTime.UtcNow
            };
        }
    }
}
