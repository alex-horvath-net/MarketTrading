using Common.Adapters.AppDataModel;
using Microsoft.EntityFrameworkCore;

namespace Common.Technology.AppData;


public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options) {
    public DbSet<Transaction> Transactions { get; set; }
}
