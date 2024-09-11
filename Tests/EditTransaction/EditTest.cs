using Common.Adapters.App.Data;
using Common.Adapters.App.Data.Model;
using Common.Business.Model;
using Common.Data.Technology;
using Common.Technology;
using Common.Technology.App.Data;
using Common.Technology.Data;
using Common.Technology.EF.App;
using Experts.Trader.EditTransaction;
using Experts.Trader.EditTransaction.Edit;
using Experts.Trader.EditTransaction.Edit.Adapters;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.EditTransaction;

public class EditTest {
    DatabaseAdapter CreateUnit() => new(dependencies.RepositoryClient);
    Task<Transaction?> UseTheUnit(DatabaseAdapter unit) => unit.EditTransaction(arguments.Request, arguments.Token);
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
        transaction.Id.Should().Be(arguments.Request.Id);
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
        services.AddRead(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var repositoryAdapterPort = serviceProvider.GetService<ICommonEFClient<TransactionDM>>();
        var repositoryTechnologyPort = serviceProvider.GetService<ICommonEFClient<TransactionDM>>();
        var ef = serviceProvider.GetService<AppDB>();

        repositoryAdapterPort.Should().NotBeNull();
        repositoryTechnologyPort.Should().NotBeNull();
        ef.Should().NotBeNull();
    }


    public record Dependencies(ICommonEFClient<TransactionDM> RepositoryClient) {

        public static Dependencies Default() {
            var dbFactory = new DatabaseFactory();
            var db = dbFactory.Default();
            var repositoryClient = new DatabaseClient<TransactionDM>(db);
            return new Dependencies(repositoryClient);
        }
    }

    public record Arguments(Request Request, CancellationToken Token) {

        public static Arguments Exists() => new(
            new() { Id = 2, Name = "EUR2" },
            CancellationToken.None);

        public static Arguments DosentExist() => new(
           new() { Id = -2, Name = "EUR2" },
           CancellationToken.None);
    }
}
