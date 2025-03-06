using YourBank.Infrastructure.OrderExecution.Models;

namespace YourBank.Infrastructure.OrderExecution.Services {
    public class OrderExecutionService : IOrderExecutionService {
        private readonly IConfiguration _configuration;
        private readonly ILogger<OrderExecutionService> _logger;

        public OrderExecutionService(IConfiguration configuration, ILogger<OrderExecutionService> logger) {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<ExecutionResponse> ProcessExecutionAsync(Models.OrderExecution execution) {
            // Log the incoming execution data.
            _logger.LogInformation("Processing execution for OrderId: {OrderId}", execution.OrderId);

            // Simulate processing delay (e.g., interacting with external exchange systems).
            int delay = _configuration.GetValue<int>("ExecutionSettings:SimulatedProcessingDelayMs");
            await Task.Delay(delay);

            // In a real system, here you'd update your order database with execution details.
            // For this demo, we simply assume the order is executed successfully.

            string defaultStatus = _configuration.GetValue<string>("ExecutionSettings:DefaultStatus");

            _logger.LogInformation("Order {OrderId} executed at price {ExecutionPrice} for quantity {ExecutedQuantity}",
                execution.OrderId, execution.ExecutionPrice, execution.ExecutedQuantity);

            return new ExecutionResponse {
                OrderId = execution.OrderId,
                Status = defaultStatus,
                Message = $"Order executed successfully at {execution.ExecutionPrice:C} for {execution.ExecutedQuantity} units on {execution.ExecutionTime}."
            };
        }
    }
}
