using OrderManagementService.Models;

namespace OrderManagementService.Services {
    public class ComplianceService : IComplianceService {
        private readonly IConfiguration _configuration;

        public ComplianceService(IConfiguration configuration) {
            _configuration = configuration;
        }

        public bool CheckOrderCompliance(Order order) {
            // For demonstration: if it's a limit order, ensure the limit price is within a reasonable range.
            if (order.OrderType == OrderType.Limit) {
                if (!order.LimitPrice.HasValue)
                    return false;

                // For example, assume the current market price is $100 (dummy value).
                // The limit price must be within ±20% of this price.
                decimal currentPrice = 100m;
                decimal lowerBound = currentPrice * 0.8m;
                decimal upperBound = currentPrice * 1.2m;
                return order.LimitPrice.Value >= lowerBound && order.LimitPrice.Value <= upperBound;
            }
            // Market orders pass compliance check by default.
            return true;
        }
    }
}
