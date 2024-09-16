using Common.Adapters.App.Data.Model;
using Common.Technology.EF.App;
using Experts.Trader.EditTransaction;
using Experts.Trader.EditTransaction.Repository.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Tests.EditTransaction.Repository.EntityFramework;

public class Driver {
    public Adapter.IClient Client;

    public Service.Request Request;
    public CancellationToken Token;

    public void DefaultDependencies() {
        var technology = CreateEfDB();
        Client = new Client(technology);
    }


    public void LightDependencies() {
        var technology = CreateListDB();
        Client = new LightRepositoryClient(technology);
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

    private List<TransactionDM> CreateListDB() {
        return new List<TransactionDM>() {
            new() { Id = 1, Name = "USD" },
            new() { Id = 2, Name = "EUR" },
            new() { Id = 3, Name = "GBD"}
        };
    }

    private AppDB CreateEfDB() {
        var dbNmae = $"test-{Guid.NewGuid()}";
        var builder = new DbContextOptionsBuilder<AppDB>().UseInMemoryDatabase(dbNmae);
        var db = new AppDB(builder.Options);
        db.Database.EnsureCreated();
        if (!db.Transactions.Any()) {
            db.Transactions.AddRange(CreateListDB());
            db.SaveChanges();
        }
        return db;
    }

    public class LightRepositoryClient(List<TransactionDM> db) : Adapter.IClient {
        public Task<List<TransactionDM>> Find(string? name, CancellationToken token) =>
            Task.FromResult(name == null ? db : db.Where(x => x.Name == name).ToList());
    }
}
