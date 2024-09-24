using Common;
using Common.Adapters.App.Data.Model;
using Common.Business.Model;
using Common.Technology.EF.App;
using Experts.Trader.FindTransactions;
using Experts.Trader.FindTransactions.Repository.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Tests.Trader.Find_Transactions;

public class Repository_Should {

    private Task<List<Transaction>> TalkTo(Service.IRepository unit) => unit.FindTransactions(Request, Token);
    public Service.IRepository Create_Unit() => new Adapter(Client);

    public Adapter.IClient Client;
    public Service.Request Request;
    public CancellationToken Token;

   

    [Fact]
    public async Task Find_Transactions() {
        Service.IRepository unit = Use_Fast_Dependencies().Create_Unit();
        List<Transaction> response = await Use_Find_All_Arguments().TalkTo(unit);
        response.Should().BeOfType<List<Transaction>>();
    }

    [Fact]
    public async Task Find_All_Transactions() {
        Service.IRepository unit = Use_Fast_Dependencies().Create_Unit();
        List<Transaction> response = await Use_Find_All_Arguments().TalkTo(unit);
        response.Should().HaveCount(totalNumberOfTransactions);
    }

    [Fact]
    public async Task Find_USD_Transactions() {
        Service.IRepository unit = Use_Fast_Dependencies().Create_Unit();
        List<Transaction> response = await Use_Find_USD_Arguments().TalkTo(unit);
        response.Count.Should().Be(1);
        response[0].Name.Should().Be("USD");
    }

    [Fact]
    public async Task Find_No_Typo_Transactions() {
        Service.IRepository unit = Use_Fast_Dependencies().Create_Unit();
        List<Transaction> response = await Use_Find_Typo_Arguments().TalkTo(unit);
        response.Should().BeEmpty();
    }


    [IntegrationFact]
    public void Use_DI() {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationManager();

        //Act    
        services.AddRepositoryAdapter(configuration);


        // Assert
        var sp = services.BuildServiceProvider();

        var adapter = sp.GetRequiredService<Service.IRepository>();
        var client = sp.GetRequiredService<Adapter.IClient>();
        var technology = sp.GetRequiredService<AppDB>();

        adapter.Should().NotBeNull();
        client.Should().NotBeNull();
        technology.Should().NotBeNull();
    }


    public Repository_Should Create_Default_Dependencies() {
        var technology = CreateEfDB();
        Client = new Client(technology);
        return this;
    }
    public Repository_Should Use_Fast_Dependencies() {
        var technology = FakeDB.Create();
        totalNumberOfTransactions = technology.Transactions.Count;
        Client = new FakeClient(technology);
        return this;
    }
    private int totalNumberOfTransactions;


    public Repository_Should Use_Find_All_Arguments() {
        Request = new() { UserId = "aladar", Name = null };
        Token = CancellationToken.None;
        return this;
    }
    public Repository_Should Use_Find_USD_Arguments() {
        Request = new() { UserId = "aladar", Name = "USD" };
        Token = CancellationToken.None;
        return this;
    }
    public Repository_Should Use_Find_Typo_Arguments() {
        Request = new() { UserId = "aladar", Name = "Typo" };
        Token = CancellationToken.None;
        return this;
    }

    private AppDB CreateEfDB() {
        var dbNmae = $"test-{Guid.NewGuid()}";
        var builder = new DbContextOptionsBuilder<AppDB>().UseInMemoryDatabase(dbNmae);
        var db = new AppDB(builder.Options);
        db.Database.EnsureCreated();
        if (!db.Transactions.Any()) {
            var fakeDB = FakeDB.Create();
            db.Transactions.AddRange(fakeDB.Transactions);
            db.SaveChanges();
        }
        return db;
    }

    public class FakeClient(FakeDB db) : Adapter.IClient {
        public Task<bool> ExistsByName(string name, CancellationToken token) => db.Transactions.Any(x => x.Name == name).ToTask();

        public Task<bool> ExistsById(long id, CancellationToken token) => db.Transactions.Any(x => x.Id == id).ToTask();

        public Task<List<TransactionDM>> Find(string? name, CancellationToken token) => name == null ? db.Transactions.ToTask() : db.Transactions.Where(x => x.Name == name).ToList().ToTask();

        public Task<TransactionDM> FindById(long id, CancellationToken token) => db.Transactions.FirstOrDefault(x => x.Id == id).ToTask();

        public async Task<TransactionDM> Update(TransactionDM model, CancellationToken token) {
            TransactionDM transaction = await FindById(model.Id, token);
            transaction.Name = model.Name;
            return transaction;
        }
    }

    public class FakeDB {
        public List<TransactionDM> Transactions { get; set; } = [];

        public static FakeDB Create() {
            var db = new FakeDB();

            db.Transactions.Add(new() { Id = 1, Name = "USD" });
            db.Transactions.Add(new() { Id = 2, Name = "EUR" });
            db.Transactions.Add(new() { Id = 3, Name = "GBD" });

            return db;
        }
    }
}