using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using TradingService.Infrastructure.Database.Models;

namespace TradingService.Infrastructure.Database;

// cd .\src\TradingService
// dotnet ef migrations add AddTradesTable -c TradingDbContext -o Infrastructure\Database\Migrations
public class TradingDbContext(DbContextOptions options, ILogger<TradingDbContext> logger) : DbContext(options) {

    public DbSet<Trade> Trades { get; set; }
    public DbSet<TradeLeg> TradeLegs { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<TradeExecutionDetail> ExecutionDetails { get; set; }
    public DbSet<EventModel> Events { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        var contextTpe = GetType();

        modelBuilder.ApplyConfigurationsFromAssembly(
            contextTpe.Assembly,
            type => type.Namespace != null &&
                             type.Namespace.StartsWith(contextTpe.Namespace));
    }



    //  if enabled lage bulk query is faster
    //  if disabled the bulk insert/update faster
    public void SetAutoDetectChanges(bool enabled = true) {
        ChangeTracker.AutoDetectChangesEnabled = enabled;
    }

    public void InspectTrackedState() {
        if (!ChangeTracker.AutoDetectChangesEnabled) {
            ChangeTracker.DetectChanges();
        }

        foreach (var entry in ChangeTracker.Entries()) {
            logger.LogDebug($"Entity: {entry.Entity.GetType().Name}, State: {entry.State}");
        }
    }


    public async Task InspectConcuency() {
        try {
            //Events: //  builder.Property(e => e.RowVersion).IsRowVersion();
            await SaveChangesAsync();
        } catch (DbUpdateConcurrencyException ex) {

            logger.LogDebug("Concurrency conflict detected!");
            foreach (var entry in ex.Entries) {
                logger.LogDebug($"Conflicted entity: {entry.Entity.GetType().Name}");
            }
        }
    }


    public Task<List<Trade>> GetTrades() => Trades
        .Include(t => t.Legs.Where(leg => leg.Quantity > 0))
        .ThenInclude(l => l.Detail)
        .Include(t => t.Tags)
        .Include(t => t.ExecutionDetail)
        .AsNoTracking()  // ChangeTracker skipped
        .AsSplitQuery()
        .Where(t => t.PortfolioCode == "P1")
        .ToListAsync();

    public Task<Dictionary<string, List<Trade>>> GetTradesGroupedByPortfolio() => Trades
            .AsNoTracking()
            .GroupBy(t => t.PortfolioCode)
            .ToDictionaryAsync(g => g.Key, g => g.ToList());
    public Task<List<Trade>> GetTradesByRawSql(string portfolioCode) => Trades
        .FromSqlInterpolated($"SELECT * FROM Trades WHERE PortfolioCode = {portfolioCode}")
        .AsNoTracking()
        .ToListAsync();

    // Shadow Property Query (LastUpdatedAt must be defined in config)
    public Task<List<Trade>> GetRecentlyUpdatedTrades(DateTime since) => Trades
        .Where(t => EF.Property<DateTime>(t, "LastUpdatedAt") >= since)
        .AsNoTracking()
        .ToListAsync();

    // Global Query Filter override
    public Task<List<Trade>> GetAllTradesIncludingDeleted() => Trades
        .IgnoreQueryFilters()
        .ToListAsync();

}
