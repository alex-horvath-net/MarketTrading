using YourBank.Infrastructure.OrderManagement.Models;

namespace YourBank.Infrastructure.OrderManagement.Services {
    public interface IComplianceService {
        // Checks whether the order complies with regulatory and internal rules.
        bool CheckOrderCompliance(Order order);
    }
}
