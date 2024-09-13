using Common.Adapters.App.Data.Model;
using Common.Technology.EF.App;
using Experts.Trader.FindTransactions;
using Experts.Trader.FindTransactions.Repository.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Tests.FindTransactions.Repository.EntityFramework;

public class Driver {
    public Adapter.IClient Client;

    public Service.Request Request;
    public CancellationToken Token;

    public void DefaultDependencies() {
        var dbNmae = $"test-{Guid.NewGuid()}";
        var builder = new DbContextOptionsBuilder<AppDB>().UseInMemoryDatabase(dbNmae);
        var db = new AppDB(builder.Options);
        db.Database.EnsureCreated();
        if (!db.Transactions.Any()) {
            db.Transactions.Add(new() { Id = 1, Name = "USD" });
            db.Transactions.Add(new() { Id = 2, Name = "EUR" });
            db.Transactions.Add(new() { Id = 3, Name = "GBD" });
            db.SaveChanges();
        }
        var technology = db;
        Client = new Client(technology);
    }

    public void LightDependencies() {
        var technology = new List<TransactionDM>() {
            new() { Id = 1, Name = "USD" },
            new() { Id = 2, Name = "EUR" },
            new() { Id = 3, Name = "GBD"}
        };
        Client = new FakeRepositoryClient(technology);
    }

    public void AllArguments() {
        Request = new() { UserId = "aladar", Name = null };
        Token = CancellationToken.None;
    }

    public void SomeArguments() {
        Request = new() { UserId = "aladar", Name = "USD" };
        Token = CancellationToken.None;
    }

    public void NothingArguments() {
        Request = new() { UserId = "aladar", Name = "USD_Typo" };
        Token = CancellationToken.None;
    }
}
