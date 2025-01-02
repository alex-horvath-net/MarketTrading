//using Common.Adapters.App.Data.Model;
//using Common.Business.Model;
//using Common.Extensions;
//using Common.Technology;
//using Common.Technology.EF.App;
//using Experts.Trader.FindTransactions;
//using Experts.Trader.FindTransactions.Feature;
//using Experts.Trader.FindTransactions.Feature.OutputPorts;
//using Microsoft.EntityFrameworkCore;

//namespace Tests.Trader.Find_Transactions;

//public class Repository_Client_Want_To {

//    private Task<List<Transaction>> Talk_To_Unit(IRepository unit) => unit.FindTransactions(Request, Token);
//    public IRepository Create_Unit() => new Repository(Client);

//    public Repository.IClient Client;
//    public Request Request;
//    public CancellationToken Token;


//    [Fact]
//    public async Task Find_Transactions() {
//        var unit = With_Fast_Dependencies().Create_Unit();
//        var response = await With_Find_All_Arguments().Talk_To_Unit(unit);
//        response.Should().BeOfType<List<Transaction>>();
//    }

//    [Fact]
//    public async Task Find_All_Transactions() {
//        var unit = With_Fast_Dependencies().Create_Unit();
//        var response = await With_Find_All_Arguments().Talk_To_Unit(unit);
//        response.Should().HaveCount(totalNumberOfTransactions);
//    }

//    [Fact]
//    public async Task Find_USD_Transactions() {
//        IRepository unit = With_Fast_Dependencies().Create_Unit();
//        List<Transaction> response = await Use_Find_USD_Arguments().Talk_To_Unit(unit);
//        response.Count.Should().Be(1);
//        response[0].Name.Should().Be("USD");
//    }

//    [Fact]
//    public async Task Find_No_Typo_Transactions() {
//        IRepository unit = With_Fast_Dependencies().Create_Unit();
//        List<Transaction> response = await Use_Find_Typo_Arguments().Talk_To_Unit(unit);
//        response.Should().BeEmpty();
//    }

//    [IntegrationFact]
//    public async Task Find_USD_Transactions2() {
//        IRepository unit = Create_Default_Dependencies().Create_Unit();
//        List<Transaction> response = await Use_Find_USD_Arguments().Talk_To_Unit(unit);
//        response.Count.Should().Be(1);
//        response[0].Name.Should().Be("USD");
//    }


//    [IntegrationFact]
//    public void Use_DI() {
//        // Arrange
//        var services = new ServiceCollection();
//        var configuration = new ConfigurationManager();
//        configuration.AddInMemoryCollection(new Dictionary<string, string?> {
//            { "ConnectionStrings:App", "" },
//            { "ConnectionStrings:Identity", "" }
//        });

//        //Act    
//        services.AddCommonTechnology(configuration);
//        services.AddRepository();


//        // Assert
//        var sp = services.BuildServiceProvider();
//        sp.GetRequiredService<IRepository>().Should().NotBeNull();
//        sp.GetRequiredService<Repository.IClient>().Should().NotBeNull();
//        sp.GetRequiredService<AppDB>().Should().NotBeNull();
//    }


//    public Repository_Client_Want_To Create_Default_Dependencies() {
//        var technology = CreateEfDB();
//        Client = new Repository.Client(technology);
//        return this;
//    }
//    public Repository_Client_Want_To With_Fast_Dependencies() {
//        var technology = FakeDB.Create();
//        totalNumberOfTransactions = technology.Transactions.Count;
//        Client = new FakeDBClient(technology);
//        return this;
//    }
//    private int totalNumberOfTransactions;


//    public Repository_Client_Want_To With_Find_All_Arguments() {
//        Request = new() { UserId = "aladar", Name = null };
//        Token = CancellationToken.None;
//        return this;
//    }
//    public Repository_Client_Want_To Use_Find_USD_Arguments() {
//        Request = new() { UserId = "aladar", Name = "USD" };
//        Token = CancellationToken.None;
//        return this;
//    }
//    public Repository_Client_Want_To Use_Find_Typo_Arguments() {
//        Request = new() { UserId = "aladar", Name = "Typo" };
//        Token = CancellationToken.None;
//        return this;
//    }

//    private AppDB CreateEfDB() {
//        var dbNmae = $"test-{Guid.NewGuid()}";
//        var builder = new DbContextOptionsBuilder<AppDB>().UseInMemoryDatabase(dbNmae);
//        var db = new AppDB(builder.Options);
//        db.Database.EnsureCreated();
//        return db;
//    }

//    public class FakeDBClient(FakeDB db) : Repository.IClient {
//        public Task<bool> ExistsByName(string name, CancellationToken token) => db.Transactions.Any(x => x.Name == name).ToTask();

//        public Task<bool> ExistsById(long id, CancellationToken token) => db.Transactions.Any(x => x.Id == id).ToTask();

//        public Task<List<TransactionDM>> Find(string? name, CancellationToken token) => name == null ? db.Transactions.ToTask() : db.Transactions.Where(x => x.Name == name).ToList().ToTask();

//        public Task<TransactionDM> FindById(long id, CancellationToken token) => db.Transactions.FirstOrDefault(x => x.Id == id).ToTask();

//        public async Task<TransactionDM> Update(TransactionDM model, CancellationToken token) {
//            TransactionDM transaction = await FindById(model.Id, token);
//            transaction.Name = model.Name;
//            return transaction;
//        }
//    }

//    public class FakeDB {
//        public List<TransactionDM> Transactions { get; set; } = [];

//        public static FakeDB Create() {
//            var db = new FakeDB();

//            db.Transactions.Add(new() { Id = 1, Name = "USD" });
//            db.Transactions.Add(new() { Id = 2, Name = "EUR" });
//            db.Transactions.Add(new() { Id = 3, Name = "GBD" });

//            return db;
//        }
//    }
//}