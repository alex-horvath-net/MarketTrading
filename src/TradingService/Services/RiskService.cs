using TradingService.Models;

namespace TradingService.Services {
    public class RiskService : IRiskService {
        private readonly IConfiguration _configuration;

        public RiskService(IConfiguration configuration) {
            _configuration = configuration;
        }

        public bool CheckOrderRisk(Order order) {
            // Simple risk check: Order quantity must not exceed the maximum allowed.
            int maxQuantity = _configuration.GetValue<int>("RiskService:MaxOrderQuantity");
            return order.Quantity <= maxQuantity;
        }
    }
}
