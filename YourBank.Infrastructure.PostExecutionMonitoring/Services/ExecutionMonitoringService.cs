using System.Collections.Concurrent;
using YourBank.Infrastructure.PostExecutionMonitoring.Models;

namespace YourBank.Infrastructure.PostExecutionMonitoring.Services {
    public class ExecutionMonitoringService : IExecutionMonitoringService {
        // Using an in-memory ConcurrentBag for demonstration.
        // In production, consider using a database for persistence.
        private readonly ConcurrentBag<ExecutionLog> _executionLogs = new();

        public Task RecordExecutionLogAsync(ExecutionLog log) {
            _executionLogs.Add(log);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<ExecutionLog>> GetExecutionLogsAsync() {
            return Task.FromResult<IEnumerable<ExecutionLog>>(_executionLogs);
        }
    }
}
