using Business.Experts.Trader.PlaceTrade;
using Business.Experts.Trader.EditTrade;
using Business.Experts.Trader.FindTrades;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Experts.Trader;
public record Trader(
    PlaceTrade.IFeatureAdapter PlaceTrade,
    FindTrades.IFeatureAdapter FindTrades,
    EditTrade.IEditTransaction EditTrade);


public static class ExpertExtensions {
    public static IServiceCollection AddTrader(this IServiceCollection services, ConfigurationManager config) => services
        .AddScoped<Trader>()
        .AddPlaceTrade(config)
        .AddFindTrade(config)
        .AddEditTrade(config);

}


// TODO: Implement the following features:

//🎯 Trader Intentions / Activities
//📈 Strategy & Preparation
// Analyze historical price and volume data

// Backtest trading strategies

// Simulate market behavior

// View pre-trade risk estimates(e.g., margin, exposure)

// Review real-time market data(order book, ticker)

//💼 Trade Lifecycle
// Place a new trade

// Preview trade before submitting

// Confirm execution

// View trade confirmations

// Monitor open positions

// Cancel a pending trade

// Amend a trade(e.g., modify volume or price)

// Close or reverse a trade

// Roll over or carry a position forward

// Tag a trade(strategy, reason, portfolio)

//🔍 Monitoring & Status
// View list of all trades

// Filter trades by symbol, status, date, strategy

// Track trade status(submitted, executed, rejected)

// Get alerts on execution(success, delay, rejection)

// Monitor positions in real-time

// View P&L(Profit & Loss) breakdown

//🛠️ Operational Adjustments
// Adjust risk exposure manually(e.g., reduce position)

// Hedge exposure(automated or manual)

// Pause or resume trading

// Force close trades if needed

// Request manual override (escalation path)

//📊 Reporting & Analysis
// Export trade history(CSV, Excel)

// Generate end-of-day report

// Compare actual vs expected outcomes

// View trade logs with timestamps

// Audit trail for regulatory use

//👤 Personal Workflow
// Save common trade configurations(templates)

// Set up watchlists

// Customize dashboard layout

// Enable dark/light mode(UI preference)

// View and manage notifications

//📎 Optional: Collaborative Actions
// Share trade idea with PortfolioManager or RiskManager

// Request pre-trade approval

// Submit explanation or justification for a trade




//public record PlaceTradeCommand(
//    string TraderId,
//    string Instrument,
//    TradeSide Side,
//    decimal Quantity,
//    decimal? Price,
//    OrderType OrderType,
//    TimeInForce TimeInForce,
//    string? StrategyCode,
//    string? PortfolioCode,
//    string? UserComment,
//    DateTime? ExecutionRequestedForUtc);

//public interface ITradeRepository {
//    Task AddAsync(Trade trade);
//}

//public class PlaceTradeHandler {
//    private readonly ITradeRepository _repository;
//    private readonly ILogger<PlaceTradeHandler> _logger;

//    public PlaceTradeHandler(ITradeRepository repository, ILogger<PlaceTradeHandler> logger) {
//        _repository = repository;
//        _logger = logger;
//    }

//    public async Task<Guid> HandleAsync(PlaceTradeCommand cmd) {
//        var trade = new Trade(
//            cmd.TraderId,
//            cmd.Instrument,
//            cmd.Side,
//            cmd.Quantity,
//            cmd.Price,
//            cmd.OrderType,
//            cmd.TimeInForce,
//            cmd.StrategyCode,
//            cmd.PortfolioCode,
//            cmd.UserComment,
//            cmd.ExecutionRequestedForUtc);

//        await _repository.AddAsync(trade);

//        _logger.LogInformation("Trade {TradeId} placed by {TraderId}", trade.Id, trade.TraderId);

//        return trade.Id;
//    }
//}

//public class TradeEntity {
//    public Guid Id { get; set; }
//    public string TraderId { get; set; } = null!;
//    public string Instrument { get; set; } = null!;
//    public string Side { get; set; } = null!;
//    public decimal Quantity { get; set; }
//    public decimal? Price { get; set; }
//    public string OrderType { get; set; } = null!;
//    public string TimeInForce { get; set; } = null!;
//    public DateTime SubmittedAtUtc { get; set; }
//    public string? StrategyCode { get; set; }
//    public string? PortfolioCode { get; set; }
//    public string? UserComment { get; set; }
//    public DateTime? ExecutionRequestedForUtc { get; set; }
//    public string Status { get; set; } = null!;
//}

//public class DomainEventEntity {
//    public Guid Id { get; set; }
//    public string EventType { get; set; } = null!;
//    public string PayloadJson { get; set; } = null!;
//    public DateTime OccurredAtUtc { get; set; }
//    public bool Processed { get; set; } = false;
//}

//public class TradingDbContext : DbContext {
//    public DbSet<TradeEntity> Trades => Set<TradeEntity>();
//    public DbSet<DomainEventEntity> DomainEvents => Set<DomainEventEntity>();

//    public TradingDbContext(DbContextOptions<TradingDbContext> options) : base(options) { }
//}

//public class EfTradeRepository : ITradeRepository {
//    private readonly TradingDbContext _db;

//    public EfTradeRepository(TradingDbContext db) {
//        _db = db;
//    }

//    public async Task AddAsync(Trade trade) {
//        var entity = new TradeEntity {
//            Id = trade.Id,
//            TraderId = trade.TraderId,
//            Instrument = trade.Instrument,
//            Side = trade.Side.ToString(),
//            Quantity = trade.Quantity,
//            Price = trade.Price,
//            OrderType = trade.OrderType.ToString(),
//            TimeInForce = trade.TimeInForce.ToString(),
//            SubmittedAtUtc = trade.SubmittedAtUtc,
//            StrategyCode = trade.StrategyCode,
//            PortfolioCode = trade.PortfolioCode,
//            UserComment = trade.UserComment,
//            ExecutionRequestedForUtc = trade.ExecutionRequestedForUtc,
//            Status = trade.Status.ToString()
//        };

//        _db.Trades.Add(entity);

//        foreach (var domainEvent in trade.DomainEvents) {
//            var eventEntity = new DomainEventEntity {
//                Id = Guid.NewGuid(),
//                EventType = domainEvent.GetType().Name,
//                PayloadJson = JsonSerializer.Serialize(domainEvent),
//                OccurredAtUtc = DateTime.UtcNow
//            };
//            _db.DomainEvents.Add(eventEntity);
//        }

//        await _db.SaveChangesAsync();
//        trade.ClearDomainEvents();
//    }
//}

//public interface IDomainEvent { }
