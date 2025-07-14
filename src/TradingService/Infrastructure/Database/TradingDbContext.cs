using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using TradingService.Domain.Orders.AggregetRoot;
using TradingService.Infrastructure.Database.Models;

namespace TradingService.Infrastructure.Database;

// cd .\src\TradingService
// dotnet ef migrations add AddTradesTable -c TradingDbContext -o Infrastructure\Database\Migrations
public class TradingDbContext : DbContext {
    public TradingDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Trade> Trades { get; set; }
    public DbSet<EventModel> Events { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        var trades = new List<Trade>() {
            new () { Id = Guid.Parse("E170BA5F-98D8-4893-8333-E21C5C79DC01"), PortfolioCode = "P1", StrategyCode = "S1", Instrument = "USD", Side = TradeSide.Buy, OrderType = OrderType.Market, Quantity = 1, Price = 100, TraderId = "me", UserComment ="" },
            new () { Id = Guid.Parse("E170BA5F-98D8-4893-8333-E21C5C79DC02"), PortfolioCode = "P1", StrategyCode = "S1", Instrument = "EUR", Side = TradeSide.Buy, OrderType = OrderType.Market, Quantity = 1, Price = 100, TraderId = "me", UserComment ="" },
            new () { Id = Guid.Parse("E170BA5F-98D8-4893-8333-E21C5C79DC03"), PortfolioCode = "P1", StrategyCode = "S1", Instrument = "GBD", Side = TradeSide.Buy, OrderType = OrderType.Market, Quantity = 1, Price = 100, TraderId = "me", UserComment ="" }
        };
        modelBuilder.Entity<Trade>()
            .HasData(trades);

        modelBuilder.Entity<EventModel>(b => {
            b.ToTable("Events");

            b.HasKey(e => new { e.Id, e.SequenceNumber }); // supports GetEvents and AppendEvent in EventStore

            b.Property(e => e.RaisedAt).HasColumnType("datetime2(7)");
            b.Property(e => e.EventTypeName).HasMaxLength(256).IsRequired();
            b.Property(e => e.RowVersion).IsRowVersion();
        });

    }
}

private struct PlaceOrderAsyncStateMachine : IAsyncStateMachine {
    public int _state;
    public AsyncTaskMethodBuilder _builder;

    public string customerName;
    public RewrittenOrderService _this;

    private Order _order;
    private TaskAwaiter _dbAwaiter;
    private TaskAwaiter _sbAwaiter;

    public void MoveNext() {
        try {
            if (_state == -1) {
                _order = new Order {
                    Id = Guid.NewGuid(),
                    CustomerName = customerName,
                    CreatedAt = DateTime.UtcNow
                };

                var dbTask = _this._db.SaveAsync(_order);
                _dbAwaiter = dbTask.GetAwaiter();

                if (!_dbAwaiter.IsCompleted) {
                    _state = 0;
                    _builder.AwaitUnsafeOnCompleted(ref _dbAwaiter, ref this);
                    return;
                }

                _dbAwaiter.GetResult();
            }

            if (_state == 0) {
                _dbAwaiter.GetResult();
            }

            var sbTask = _this._sb.SendMessageAsync("Ordered");
            _sbAwaiter = sbTask.GetAwaiter();

            if (!_sbAwaiter.IsCompleted) {
                _state = 1;
                _builder.AwaitUnsafeOnCompleted(ref _sbAwaiter, ref this);
                return;
            }

            _sbAwaiter.GetResult();
        } catch (Exception ex) {
            _builder.SetException(ex);
            return;
        }

        _builder.SetResult();
    }

    public void SetStateMachine(IAsyncStateMachine stateMachine) {
        _builder.SetStateMachine(stateMachine);
    }
}
    

