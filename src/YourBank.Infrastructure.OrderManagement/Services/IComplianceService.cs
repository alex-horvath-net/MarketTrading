using OrderManagementService.Models;

namespace OrderManagementService.Services {
    public interface IComplianceService {
        // Checks whether the order complies with regulatory and internal rules.
        bool CheckOrderCompliance(Order order);
    }
}
