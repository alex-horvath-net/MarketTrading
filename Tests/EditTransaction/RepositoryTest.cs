using Common.Adapters.App.Data;
using Common.Adapters.App.Data.Model;
using Common.Business.Model;
using Common.Technology.EF.App;
using Experts.Trader.EditTransaction;
using Experts.Trader.EditTransaction.Repository.EntityFramework;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.EditTransaction;

public class RepositoryTest {
    Adapter CreateUnit() => new(
        dependencies.Client);
    
    Task<Transaction?> UseTheUnit(Adapter unit) => unit.EditTransaction(
        arguments.Request, 
        arguments.Token);
    
    Dependencies dependencies = Dependencies.Default();
    Arguments arguments = Arguments.Exists();

    [Fact]
    public async Task It_Should_Provide_Null_If_It_Dosnnt_Exists() {
        var unit = CreateUnit();
        arguments = Arguments.DosentExist();
        var transaction = await UseTheUnit(unit);
        transaction.Should().BeNull();
    }
    
    [Fact]
    public async Task It_Should_Provide_Transaction() {
        var unit = CreateUnit(); 
        var transaction = await UseTheUnit(unit);
        transaction.Should().BeOfType<Transaction>();
    }

    
    [Fact]
    public async Task It_Should_Provide_The_Matching_Transaction_If_It_Exists() {
        var unit = CreateUnit();
        var transaction = await UseTheUnit(unit);
        transaction.Id.Should().Be(arguments.Request.TransactionId);
    }


    [Fact]
    public async Task It_Should_Update_The_Name_Of_Transaction() {
        var unit = CreateUnit();
        var transaction = await UseTheUnit(unit);
        transaction.Name.Should().Be(arguments.Request.Name);
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
        services.AddRepository(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var repositoryAdapterPort = serviceProvider.GetService<Service.IRepository>();
        var repositoryTechnologyPort = serviceProvider.GetService<Adapter.IClient>();
        var ef = serviceProvider.GetService<AppDB>();

        repositoryAdapterPort.Should().NotBeNull();
        repositoryTechnologyPort.Should().NotBeNull();
        ef.Should().NotBeNull();
    }


    public record Dependencies(Adapter.IClient Client) {

        public static Dependencies Default() {
            var db = DatabaseFactory.Default();
            var repositoryClient = new Client(db);
            return new Dependencies(repositoryClient);
        }
    }

    public record Arguments(Service.Request Request, CancellationToken Token) {

        public static Arguments Exists() => new(
            new() { UserId = "aladar", TransactionId = 2, Name = "EUR2" },
            CancellationToken.None);

        public static Arguments DosentExist() => new(
           new() { TransactionId = -2, Name = "EUR2" },
           CancellationToken.None);
    }
}
