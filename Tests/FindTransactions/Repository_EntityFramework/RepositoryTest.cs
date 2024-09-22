using Common;
using Common.Adapters.App.Data.Model;
using Common.Business.Model;
using Common.Technology.EF.App;
using Experts.Trader.FindTransactions;
using Experts.Trader.FindTransactions.Repository.EntityFramework;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.FindTransactions.Repository_EntityFramework;

public class RepositoryTest {

    public Adapter.IClient? Client;
    public Service.IRepository? Unit;
    public void Create_The_Unit() => Unit = new Adapter(Client);

    public Service.Request? Request;
    public CancellationToken Token;
    public List<Transaction>? Response;
    public async Task Use_The_Unit() => Response = await Unit.FindTransactions(Request, Token);

    [Fact]
    public async Task Find_Transactions() {
        Create_Fast_Dependencies();
        Create_The_Unit();
        Create_Find_All_Arguments();
        await Use_The_Unit();
        Response.Should().BeOfType<List<Transaction>>();
    }

    [Fact]
    public async Task Find_All_Transactions() {
        Create_Fast_Dependencies();
        Create_The_Unit();
        Create_Find_All_Arguments();
        await Use_The_Unit();
        Response.Count.Should().Be(countOfTransactions);
    } 

    [Fact]
    public async Task Find_USD_Transactions() {
        Create_Fast_Dependencies();
        Create_The_Unit();
        Create_Find_USD_Arguments();
        await Use_The_Unit(); 
        Response.Count.Should().Be(1);
        Response[0].Name.Should().Be("USD");
    }

    [Fact]
    public async Task Find_Typo_Transactions() {
        Create_Fast_Dependencies();
        Create_The_Unit();
        Create_Find_Typo_Arguments();
        await Use_The_Unit(); 
        Response.Count.Should().Be(0);
    }


    [Fact]
    public void Register_Dependencies() { 
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationManager();
       
        //Act    
        services.AddRepositoryAdapter(configuration);


        // Assert
        var sp = services.BuildServiceProvider();
       
        var adapter = sp.GetService<Service.IRepository>();
        var client = sp.GetService<Adapter.IClient>();
        var technology = sp.GetService<AppDB>();

        adapter.Should().NotBeNull();
        client.Should().NotBeNull();
        technology.Should().NotBeNull();
    }


    public RepositoryTest Create_Default_Dependencies() {
        var technology = CreateEfDB();
        Client = new Client(technology);
        return this;
    }
    public RepositoryTest Create_Fast_Dependencies() {
        var technology = FakeDB.Create();
        countOfTransactions = technology.Transactions.Count;
        Client = new FakeClient(technology);
        return this;
    }
    private int countOfTransactions;


  
    public void Create_Find_All_Arguments() {
        Request = new() { UserId = "aladar", Name = null };
        Token = CancellationToken.None;
    }
    public void Create_Find_USD_Arguments() {
        Request = new() { UserId = "aladar", Name = "USD" };
        Token = CancellationToken.None;
    }
    public void Create_Find_Typo_Arguments() {
        Request = new() { UserId = "aladar", Name = "Typo" };
        Token = CancellationToken.None;
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