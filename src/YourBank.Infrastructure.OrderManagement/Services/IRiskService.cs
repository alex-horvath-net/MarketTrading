using YourBank.Infrastructure.OrderManagement.Models;

namespace YourBank.Infrastructure.OrderManagement.Services {
    public interface IRiskService {
        // Checks whether the order meets predefined risk limits.
        bool CheckOrderRisk(Order order);
    }
}
