using Infrastructure.Data.App.Model;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.App;


public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options) {
    public DbSet<Transaction> Transactions { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Transaction>().HasData(
            new Transaction { Id = 1 },
            new Transaction { Id = 2 },
            new Transaction { Id = 3 } 
        );
    }
}
