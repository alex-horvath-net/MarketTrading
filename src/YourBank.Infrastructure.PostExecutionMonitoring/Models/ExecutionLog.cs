namespace PostExecutionMonitoringService.Models {
    public class ExecutionLog {
        public Guid OrderId { get; set; }              // Unique identifier for the order.
        public string Symbol { get; set; } = string.Empty; // The asset symbol (e.g., AAPL).
        public int ExecutedQuantity { get; set; }        // Number of units executed.
        public decimal ExecutionPrice { get; set; }      // Price at which the order was executed.
        public DateTime ExecutionTime { get; set; }      // Timestamp of execution.
        public string Status { get; set; } = string.Empty; // Final order status (e.g., Executed, Partially Executed).
        public string Message { get; set; } = string.Empty;  // Additional execution details.
    }
}
