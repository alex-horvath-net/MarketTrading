using PostExecutionMonitoringService.Models;

namespace PostExecutionMonitoringService.Services {
    public interface IExecutionMonitoringService {
        Task RecordExecutionLogAsync(ExecutionLog log);
        Task<IEnumerable<ExecutionLog>> GetExecutionLogsAsync();
    }
}
