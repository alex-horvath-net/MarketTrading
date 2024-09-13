using Common.Adapters.App.Data.Model;
using Common.Business.Model;
using Common.Technology.EF.App;
using Experts.Trader.FindTransactions;
using Experts.Trader.FindTransactions.Repository.EntityFramework;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.FindTransactions.Repository.EntityFramework;

public class RepositoryTest : Driver {
    [Fact]
    public async Task It_Should_Find_Somthing() {
        LightDependencies();
        var unit = CreateUnit();
        AllArguments();
        var transactions = await UseTheUnit(unit);
        transactions.Should().NotBeNull();
    }

    [Fact]
    public async Task It_Should_Find_List_Of_Transaction() {
        LightDependencies();
        var unit = CreateUnit();
        AllArguments();
        var transactions = await UseTheUnit(unit);
        transactions.Should().BeOfType<List<Transaction>>();
    }

    [Fact]
    public async Task It_Should_Find_All_Transaction() {
        LightDependencies();
        var unit = CreateUnit();
        AllArguments();
        var transactions = await UseTheUnit(unit);
        transactions.Should().NotBeEmpty();
    }

    [Fact]
    public async Task It_Should_Find_Some_Transaction() {
        LightDependencies();
        var unit = CreateUnit();
        SomeArguments();
        var transactions = await UseTheUnit(unit);
        transactions.Should().NotBeEmpty();
    }

    [Fact]
    public async Task It_Should_Find_Some_Matching_Transaction() {
        LightDependencies();
        var unit = CreateUnit();
        SomeArguments();
        var transactions = await UseTheUnit(unit);
        transactions.Should().AllSatisfy(x => x.Name.Contains(Request.Name));
    }

    [Fact]
    public async Task It_Should_Not_Find_Non_Matching_Transactions() {
        LightDependencies();
        var unit = CreateUnit();
        NothingArguments();
        var transactions = await UseTheUnit(unit);
        transactions.Should().BeEmpty();
    }

    [Fact]
    public void AddRead_ShouldRegisterDependencies() {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationManager();
        configuration.AddInMemoryCollection(new Dictionary<string, string?> {
            { "ConnectionStrings:App", "Data Source=.\\SQLEXPRESS;Initial Catalog=App;User ID=sa;Password=sa!Password;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False" }
        });
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
}

public class Driver {
    public Adapter CreateUnit() => new(Client);

    public Task<List<Transaction>> UseTheUnit(Adapter unit) => unit.FindTransactions(Request, Token);

    public Adapter.IClient Client;

    public Service.Request Request;
    public CancellationToken Token;

    public void DefaultDependencies() { 
        var technology = DatabaseFactory.Default();
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

public class FakeRepositoryClient(List<TransactionDM> fakeDB) :Adapter.IClient {
    public Task<List<TransactionDM>> Find(string? name, CancellationToken token) => Task.FromResult(
        fakeDB.Where(x => x.Name == name).ToList());
}