namespace TradingService.Infrastructure.Database.Models;

// Used for classifying trades by strategy, risk level, etc.
public class Tag {
    public int Id { get; set; }
    public string Name { get; set; } = default!;
        
    public ICollection<Trade> Trades { get; set; } = new List<Trade>(); // *:*
}
