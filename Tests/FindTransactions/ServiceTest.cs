using Experts.Trader.FindTransactions;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tests.FindTransactions.Repository_EntityFramework;
using Tests.FindTransactions.Validator_FluentValidator;

namespace Tests.FindTransactions;

public class ServiceTest {

    public Service.IValidator Validator;
    public Service.IFlag Flag;
    public Service.IRepository Repository;
    public Service.IClock Clock;
    public Service Unit;
    public void Create_Unit() => Unit = new Service(Validator, Flag, Repository, Clock);


    public Service.Response Response;
    public Service.Request Request;
    public CancellationToken Token;
    public async Task Use_The_Unit() =>  Response = await Unit.Execute(Request, Token);

    [Fact]
    public async Task Response_Should_NotBeNull() {
        Create_Fast_Dependencies();
        Create_Unit();
        Create_Valid_Arguments();
        await Use_The_Unit();
        Response.Should().NotBeNull();
    }

    [Fact]
    public async Task Response_Request_Should_NotBeNull() {
        Create_Fast_Dependencies();
        Create_Unit();
        Create_Valid_Arguments();
        await Use_The_Unit();
        Response.Request.Should().NotBeNull();
    }

    [Fact]
    public async Task Response_Errors_Should_Reflect_Validation_Issues() {
        Create_Fast_Dependencies();
        Create_Unit();
        Create_Non_Valid_Arguments();
        await Use_The_Unit();
        Response.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Response_Transactions_Should_BeEmpty_If_There_Is_Validation_Issues() {
        Create_Fast_Dependencies();
        Create_Unit();
        Create_Non_Valid_Arguments();
        await Use_The_Unit();
        Response.Transactions.Should().BeEmpty();
    }

    [Fact]
    public void AddRead_ShouldRegisterDependencies() {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationManager();
        //configuration.AddInMemoryCollection(new Dictionary<string, string?> {
        //    { "ConnectionStrings:App", "Data Source=.\\SQLEXPRESS;Initial Catalog=App;User ID=sa;Password=sa!Password;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False" }
        //});
        // Act
        services.AddFindTransactions(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var feature = serviceProvider.GetService<Service>();

        feature.Should().NotBeNull();
    }


    public void Create_Fast_Dependencies() {
        RepositoryTest.Create_Fast_Dependencies().Create_The_Unit();
        Repository = RepositoryTest.Unit;

        ValidatorTest.Create_Default_Dependencies().Create_The_Unit();
        Validator = ValidatorTest.Unit;

        var flagClient = new Experts.Trader.FindTransactions.Flag.Microsoft.Client();
        Flag = new Experts.Trader.FindTransactions.Flag.Microsoft.Adapter(flagClient);
        
        var clockClient = new Experts.Trader.FindTransactions.Clock.Microsoft.Client();
        Clock = new Experts.Trader.FindTransactions.Clock.Microsoft.Adapter(clockClient);
    }


    public void Create_Valid_Arguments() {
        ValidatorTest.Create_Valid_Arguments();
        Request = ValidatorTest.Request;
        Token = ValidatorTest.Token;
    }

    public void Create_Non_Valid_Arguments() {
        ValidatorTest.Create_Non_Valid_Arguments();
        Request = ValidatorTest.Request;
        Token = ValidatorTest.Token;
    }


    public ValidatorTest ValidatorTest = new();
    public RepositoryTest RepositoryTest = new();
}

