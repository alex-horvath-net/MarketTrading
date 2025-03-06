using YourBank.Infrastructure.PostExecutionMonitoring.Models;

namespace YourBank.Infrastructure.PostExecutionMonitoring.Services {
    public interface IExecutionMonitoringService {
        Task RecordExecutionLogAsync(ExecutionLog log);
        Task<IEnumerable<ExecutionLog>> GetExecutionLogsAsync();
    }
}
