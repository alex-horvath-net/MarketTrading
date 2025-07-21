using MarketDataIngestorService.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketDataIngestorService.Infrastructure; 
public class IngestorDbConctext(DbContextOptions<IngestorDbConctext> options) : DbContext(options) {
    public DbSet<BloombergSymbol> BloombergSymbols => Set<BloombergSymbol>();
}