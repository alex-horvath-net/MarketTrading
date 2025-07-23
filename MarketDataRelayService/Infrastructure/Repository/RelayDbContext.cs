using MarketDataRelayService.Infrastructure.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketDataRelayService.Infrastructure.Repository; 
public class RelayDbContext(DbContextOptions<RelayDbContext> options) : DbContext(options) {
    public DbSet<MarketPriceEntity> MarketPrice => Set<MarketPriceEntity>();
}