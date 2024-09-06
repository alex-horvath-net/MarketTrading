using Common.Data.Technology;
using Microsoft.EntityFrameworkCore;

namespace Tests;
public  class DatabaseFactory {

    public  AppDB Empty() {
        var dbNmae = $"test-{Guid.NewGuid()}";
        var builder = new DbContextOptionsBuilder<AppDB>().UseInMemoryDatabase(dbNmae);
        var db = new AppDB(builder.Options);
        db.Database.EnsureCreated();
        return db;
    }

    public  AppDB Default() {

        var db = Empty();
        if (db !=null && db.Transactions.Any())
            return db;

        db.Transactions.Add(new() { Id = 1, Name = "USD" });
        db.Transactions.Add(new() { Id = 2, Name = "EUR" });
        db.Transactions.Add(new() { Id = 3, Name = "GBD"});
        db.SaveChanges();

        return db;
    }
}