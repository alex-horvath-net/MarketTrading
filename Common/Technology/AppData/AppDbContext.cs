using Common.Adapters.AppDataModel;
using Microsoft.EntityFrameworkCore;

namespace Common.Technology.AppData;


public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options) {
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Transaction>().HasData(
            new() { Id = 1, Name = "USD" },
            new() { Id = 2, Name = "EUR" },
            new() { Id = 3, Name = "GBD" });
    }
}
