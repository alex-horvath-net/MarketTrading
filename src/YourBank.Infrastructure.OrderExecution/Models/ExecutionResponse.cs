namespace YourBank.Infrastructure.OrderExecution.Models {
    // Represents the confirmation response sent back to the client.
    public class ExecutionResponse {
        public Guid OrderId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
