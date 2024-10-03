using Common;
using Common.Adapters.App.Data.Model;
using Common.Business.Model;
using Common.Technology;
using Common.Technology.EF.App;
using Experts.Trader.EditTransaction;
using Experts.Trader.EditTransaction.WorkSteps;
using Microsoft.EntityFrameworkCore;

namespace Tests.Trader.Edit_Transaction;

public class Repository_Should {

    public Repository.BusinessAdapter.ITechnologyAdapter TechnologyAdapter;
    public BusinessNeed.IRepository Unit;
    public BusinessNeed.IRepository Create_The_Unit() => Unit = new Repository.BusinessAdapter(TechnologyAdapter);

    public Transaction Response;
    public BusinessNeed.Request Request;
    public CancellationToken Token;
    public async Task Use_The_Unit() => Response = await Unit.EditTransaction(Request, Token);


    [Xunit.Fact]
    public async Task Present_Transaction() {
        Create_Fast_Dependencies();
        Create_The_Unit();
        Create_Name_Chager_Arguments();
        await Use_The_Unit();
        Response.Should().NotBeNull();
        Response.Should().BeOfType<Transaction>();
    }

    [Xunit.Fact]
    public async Task Present_The_Rright_Transaction() {
        Create_Fast_Dependencies();
        Create_The_Unit();
        Create_Name_Chager_Arguments();
        await Use_The_Unit();
        Response.Id.Should().Be(eurTansactionId);
    }

    [Xunit.Fact]
    public async Task Present_The_Transaction_With_Updated_Name() {
        Create_Fast_Dependencies();
        Create_The_Unit();
        Create_Name_Chager_Arguments();
        await Use_The_Unit();
        Response.Name.Should().Be(eurNewName);
    }

    [IntegrationFactAttribute]
    public void Use_DI() {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationManager();
        configuration.AddInMemoryCollection(new Dictionary<string, string?> {
            { "ConnectionStrings:App", "" },
            { "ConnectionStrings:Identity", "" }
        });
        // Act
        services.AddCommonTechnology(configuration);
        services.AddRepositoryAdapter();
        var sp = services.BuildServiceProvider();

        // Assert
        sp.GetRequiredService<BusinessNeed.IRepository>().Should().NotBeNull();
        sp.GetRequiredService<Repository.BusinessAdapter.ITechnologyAdapter>().Should().NotBeNull();
        sp.GetRequiredService<AppDB>().Should().NotBeNull();
    }


    public Repository_Should Create_Default_Dependencies() {
        var technology = CreateEfDB();
        TechnologyAdapter = new Repository.TechnologyAdapter(technology);
        return this;
    }
    public void Create_Fast_Dependencies() {
        var technology = FakeDB.Create();
        TechnologyAdapter = new FakeTechnologyAdapter(technology);
    }


    public (BusinessNeed.Request, CancellationToken) Create_Name_Chager_Arguments() {
        Request = new() { TransactionId = eurTansactionId, Name = eurNewName };
        Token = CancellationToken.None;
        return (Request, Token);
    }
    private long eurTansactionId = 2;
    private string eurNewName = "EUR2";


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

    public class FakeTechnologyAdapter(FakeDB db) : Repository.BusinessAdapter.ITechnologyAdapter {
        public Task<bool> NameIsUnique(string name, CancellationToken token) => db.Transactions.All(x => x.Name != name).ToTask();

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
