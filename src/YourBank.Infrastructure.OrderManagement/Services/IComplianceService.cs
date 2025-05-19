using TradingService.Models;

namespace TradingService.Services {
    public interface IComplianceService {
        // Checks whether the order complies with regulatory and internal rules.
        bool CheckOrderCompliance(Order order);
    }
}
