using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Infrastructure.Adapters.App.Data.Model;

namespace Infrastructure.Technology.EF;

// Add-Migration InitAppDB -c AppDB -o "Technology/EF/Migrations/AppDBMigrations"
// Update-Database -c AppDB -v
public class AppDB : DbContext {
    // public AppDB() : base() { }
    public AppDB(DbContextOptions<AppDB> options, IConfiguration configuration = null) : base(options) {
        this.configuration = configuration;
    }
    private readonly IConfiguration configuration;
    public DbSet<Trade> Trades { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (!optionsBuilder.IsConfigured) {
            var coonectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            optionsBuilder.UseSqlServer(coonectionString);
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Trade>().HasData(
            new() { Id = Guid.NewGuid(), Instrument = "USD", Side = TradeSide.Buy, OrderType = OrderType.Market, Quantity = 1, Price = 100, TraderId = "me" },
            new() { Id = Guid.NewGuid(), Instrument = "EUR", Side = TradeSide.Buy, OrderType = OrderType.Market, Quantity = 1, Price = 100, TraderId = "me" },
            new() { Id = Guid.NewGuid(), Instrument = "GBD", Side = TradeSide.Buy, OrderType = OrderType.Market, Quantity = 1, Price = 100, TraderId = "me" });
    }

    public override void Dispose() {
        base.Dispose();
    }

    public override ValueTask DisposeAsync() {
        return base.DisposeAsync();
    }
}

public class OutboxMessage {
    public Guid Id { get; set; }                      // Message ID
    public Guid TransactionId { get; set; }           // Local DB transaction for producing the message
    public Guid CorrelationId { get; set; }           // End-to-end business operation ID (shared)
    public DateTime OccurredOn { get; set; }          // Time of event
    public string Type { get; set; } = default!;      // Full .NET type name
    public string Payload { get; set; } = default!;   // Serialized JSON body
    public string Status { get; set; } = "Created";   // Created, Sent, Failed
    public DateTime? LastAttemptedOn { get; set; }    // Retry time
    public int RetryCount { get; set; } = 0;          // Retry attempts
    public string? Error { get; set; }                // Optional error info
}

public class InboxMessage {
    public Guid Id { get; set; }                      // Message ID (matches producer’s ID)
    public Guid TransactionId { get; set; }           // Local DB transaction for processing the message
    public Guid CorrelationId { get; set; }           // Shared business ID from producer
    public DateTime ReceivedOn { get; set; }          // Time received
    public string Type { get; set; } = default!;      // Optional .NET type info
    public DateTime? ProcessedOn { get; set; }        // Time of successful handling
    public string? Error { get; set; }                // Error message (if failed)
}





