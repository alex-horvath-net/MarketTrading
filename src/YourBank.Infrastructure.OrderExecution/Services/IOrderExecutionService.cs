using YourBank.Infrastructure.OrderExecution.Models;

namespace YourBank.Infrastructure.OrderExecution.Services {
    public interface IOrderExecutionService {
        Task<ExecutionResponse> ProcessExecutionAsync(Models.OrderExecution execution);
    }
}
