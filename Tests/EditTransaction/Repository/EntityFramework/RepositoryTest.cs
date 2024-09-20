using AngleSharp.Io;
using Common;
using Common.Adapters.App.Data.Model;
using Common.Business.Model;
using Common.Technology.EF.App;
using Experts.Trader.EditTransaction;
using Experts.Trader.EditTransaction.Repository.EntityFramework;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.EditTransaction.Repository.EntityFramework;

public class RepositoryTest {
    public Service.IRepository CreateUnit() => new Adapter(Client);
    public Task<Transaction> Run(Service.IRepository unit) => unit.EditTransaction(Request, Token);

    [Fact]
    public async Task Response_Should_Be_Presented() {
        var dependecies = CreateFastDependencies();
        var unit = CreateUnit();
        var arguments = SetChageRequest();
        var response = await Run(unit);
        response.Should().NotBeNull();
    }

    [Fact]
    public async Task Response_Should_Be_A_Transaction() {
        var dependecies = CreateFastDependencies();
        var unit = CreateUnit();
        var arguments = SetChageRequest();
        var response = await Run(unit);
        response.Should().BeOfType<Transaction>();
    }

    [Fact]
    public async Task Response_Should_Match_With_Request_Id() {
        var dependecies = CreateFastDependencies();
        var unit = CreateUnit();
        var arguments = SetChageRequest();
        var response = await Run(unit);
        response.Id.Should().Be(eurTansactionId);
    }

    [Fact]
    public async Task Response_Should_Match_With_Request_Name() {
        var dependecies = CreateFastDependencies();
        var unit = CreateUnit();
        var arguments = SetChageRequest();
        var response = await Run(unit);
        response.Name.Should().Be(eurNewName);
    }

    [Fact]
    public void DI_ShouldRegisterDependencies() {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationManager();
        //configuration.AddInMemoryCollection(new Dictionary<string, string?> {
        //    { "ConnectionStrings:App", "Data Source=.\\SQLEXPRESS;Initial Catalog=App;User ID=sa;Password=sa!Password;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False" }
        //});
        // Act
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


    public Adapter.IClient Client;
    public RepositoryTest CreateDefaultDependencies() {
        var technology = CreateEfDB();
        Client = new Client(technology);
        return this;
    }
    public (Adapter.IClient Client, Adapter.IClient _) CreateFastDependencies() {
        var technology = FakeDB.Create();
        Client = new FakeClient(technology);
        return (Client, Client);
    }


    public Service.Request Request;
    public CancellationToken Token;
    public (Service.Request, CancellationToken) SetChageRequest() {
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