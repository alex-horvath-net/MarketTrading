using MarketDataRelayService.Domain;
using MarketDataRelayService.Features.RelayLiveMarketData;
using MarketDataRelayService.Infrastructure.Repository.Models;

namespace MarketDataRelayService.Infrastructure.Repository;

public class SqlMarketDataStorage : IMarketDataStorage {
    private readonly RelayDbContext _dbContext;
    private readonly ILogger<SqlMarketDataStorage> _logger;

    public SqlMarketDataStorage(RelayDbContext db, ILogger<SqlMarketDataStorage> logger) {
        _dbContext = db;
        _logger = logger;
    }

    public async Task StoreAsync(MarketPrice price, CancellationToken token) {
        try {
            var entry = new MarketPriceEntity {
                Symbol = price.Symbol,
                Timestamp = price.Timestamp,
                Ask = price.Ask,
                Bid = price.Bid ,
                Last = price.Last,
                CorrelationId = price.CorrelationId,
            };
            _dbContext.MarketPrice.Add(entry);
            await _dbContext.SaveChangesAsync(token);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to store MarketPrice for {Symbol}", price.Symbol);
        }
    }
}