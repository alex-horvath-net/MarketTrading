namespace TradingService.Models {
    public class OrderResponse {
        public Guid OrderId { get; set; }
        public OrderStatus Status { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
