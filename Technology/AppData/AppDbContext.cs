using Adapters.AppDataModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Plugins.AppData;


public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options) {
    public DbSet<Transaction> Transactions { get; set; }
}
