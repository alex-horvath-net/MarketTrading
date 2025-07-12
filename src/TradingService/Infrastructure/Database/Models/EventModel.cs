namespace TradingService.Infrastructure.Database.Models;

public record EventModel {
    public Guid Id { get; set; }
    public int SequenceNumber { get; set; }
    public DateTime RaisedAt { get; set; }
    public string? EventTypeName { get; set; }
    public string? EventContent { get; set; }
    public byte[]? RowVersion { get; set; }
}
