using OrderManagementService.Models;

namespace OrderManagementService.Services {
    public interface IRiskService {
        // Checks whether the order meets predefined risk limits.
        bool CheckOrderRisk(Order order);
    }
}
