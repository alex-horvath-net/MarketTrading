using MonitoringService.Models;

namespace MonitoringService.Services {
    public interface IExecutionMonitoringService {
        Task RecordExecutionLogAsync(ExecutionLog log);
        Task<IEnumerable<ExecutionLog>> GetExecutionLogsAsync();
    }
}
