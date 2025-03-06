using YourBank.Infrastructure.OrderManagement.Models;

namespace YourBank.Infrastructure.OrderManagement.Services {
    public class OrderService {
        private readonly IRiskService _riskService;
        private readonly IComplianceService _complianceService;
        private readonly IConfiguration _configuration;

        public OrderService(IRiskService riskService, IComplianceService complianceService, IConfiguration configuration) {
            _riskService = riskService;
            _complianceService = complianceService;
            _configuration = configuration;
        }

        public OrderResponse PlaceOrder(Order order) {
            // Perform risk check
            if (!_riskService.CheckOrderRisk(order)) {
                return new OrderResponse {
                    OrderId = Guid.NewGuid(),
                    Status = OrderStatus.Rejected,
                    Message = "Order rejected: Quantity exceeds risk limits."
                };
            }

            // Perform compliance check
            if (!_complianceService.CheckOrderCompliance(order)) {
                return new OrderResponse {
                    OrderId = Guid.NewGuid(),
                    Status = OrderStatus.Rejected,
                    Message = "Order rejected: Compliance rules not met."
                };
            }

            // Simulate order execution. In a real system, this is where you would send the order
            // to an exchange (via FIX API, etc.) and wait for an execution report.
            return new OrderResponse {
                OrderId = Guid.NewGuid(),
                Status = OrderStatus.Executed,
                Message = "Order executed successfully."
            };
        }
    }
}
