using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Infrastructure.Adapters.App.Data.Model;

namespace Infrastructure.Technology.EF.App;


public class AppDB : DbContext {
    public AppDB() : base() { }
    public AppDB(DbContextOptions<AppDB> options, IConfiguration configuration = null) : base(options) {
        this.configuration = configuration;
    }
    private readonly IConfiguration configuration;
    public DbSet<TransactionDM> Transactions { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (!optionsBuilder.IsConfigured) {
            var coonectionString = configuration.GetConnectionString("App") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            optionsBuilder.UseSqlServer(coonectionString);
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<TransactionDM>().HasData(
            new() { Id = 1, Name = "USD" },
            new() { Id = 2, Name = "EUR" },
            new() { Id = 3, Name = "GBD" });
    }

    public override void Dispose() {
        base.Dispose();
    }

    public override ValueTask DisposeAsync() {
        return base.DisposeAsync();
    }
}


