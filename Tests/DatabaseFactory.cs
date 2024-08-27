using Common.Technology.AppData;
using Microsoft.EntityFrameworkCore;

namespace Experts.Trader.ReadTransactions.Business;
public static class DatabaseFactory {

    public static AppDbContext Empty() {
        var builder = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("test");
        var db = new AppDbContext(builder.Options);
        db.Database.EnsureCreated();
        return db;
    }

    public static AppDbContext Default() {

        var db = Empty();
        if (db.Transactions.Any())
            return db;

        db.Transactions.Add(new() { Id = 1 });
        db.Transactions.Add(new() { Id = 2 });
        db.Transactions.Add(new() { Id = 3 });
        db.SaveChanges();

        return db;
    }
}