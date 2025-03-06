namespace YourBank.Infrastructure.OrderExecution.Models {
    public class OrderExecution {
        public Guid OrderId { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public int ExecutedQuantity { get; set; }
        public decimal ExecutionPrice { get; set; }
        public DateTime ExecutionTime { get; set; }
    }
}
