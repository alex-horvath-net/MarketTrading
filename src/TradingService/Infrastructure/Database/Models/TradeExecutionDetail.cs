namespace TradingService.Infrastructure.Database.Models;

public class TradeExecutionDetail {
    public Guid Id { get; set; } // Same as Trade.Id

    public DateTime ExecutedAt { get; set; }
    public string Venue { get; set; } = default!;
    public string? ExecutionRef { get; set; }

    public Trade Trade { get; set; } = default!; //1:1
}