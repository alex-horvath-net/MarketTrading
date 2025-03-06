namespace YourBank.Infrastructure.OrderManagement.Models {
    public class Order {
        public string Symbol { get; set; } = string.Empty;
        public OrderType OrderType { get; set; }
        public int Quantity { get; set; }
        public decimal? LimitPrice { get; set; } // Applicable for limit orders
    }
}
