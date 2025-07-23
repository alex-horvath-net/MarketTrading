using MarketDataIngestionService.Infrastructure.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketDataIngestionService.Infrastructure.Database; 
public class IngestorDbContext(DbContextOptions<IngestorDbContext> options) : DbContext(options) {
    public DbSet<BloombergSymbol> Symbols => Set<BloombergSymbol>();
}