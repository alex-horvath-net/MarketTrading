using Experts.Trader.EditTransaction;
using EntityFramework = Experts.Trader.EditTransaction.Repository.EntityFramework;
using FluentValidator = Experts.Trader.EditTransaction.Validator.FluentValidator;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tests.EditTransaction.Repository_EntityFramework;

namespace Tests.EditTransaction;

public class ServiceTest {
    public Service.IValidator Validator;
    public Service.IRepository Repository;
    public Service Unit;
    public void Crea_The_Unit() => Unit = new(Validator, Repository);

    public Service.Response Response;
    public Service.Request Request;
    public CancellationToken Token;
    public async Task Use_The_Unit() => Response = await Unit.Execute(Request, Token);


    [Fact]
    public async Task Response_Should_NotBeNull() {
        Create_Fast_Dependencies();
        Crea_The_Unit();
        Create_Valid_Arguments();
        await Use_The_Unit();
        Response.Should().NotBeNull();
    }

    [Fact]
    public async Task Response_Request_Should_NotBeNull() {
        Create_Fast_Dependencies();
        Crea_The_Unit();
        Create_Valid_Arguments();
        await Use_The_Unit();
        Response.Request.Should().NotBeNull();
    }

    [Fact]
    public async Task Response_Errors_Should_Reflect_Validation_Issues() {
        Create_Fast_Dependencies();
        Crea_The_Unit();
        Create_Non_Valid_Arguments();
        await Use_The_Unit();
        Response.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Response_Transactions_Should_BeNull_If_There_Is_Validation_Issues() {
        Create_Fast_Dependencies();
        Crea_The_Unit();
        Create_Non_Valid_Arguments();
        await Use_The_Unit();
        Response.Transaction.Should().BeNull();
    }


    [Fact]
    public void AddEditTransaction_ShouldRegisterDependencies() {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationManager();
        configuration.AddInMemoryCollection(new Dictionary<string, string?> {
            { "ConnectionStrings:App", "Data Source=.\\SQLEXPRESS;Initial Catalog=App;User ID=sa;Password=sa!Password;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False" }
        });
        // Act
        services.AddEditTransaction(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var feature = serviceProvider.GetService<Service>();

        feature.Should().NotBeNull();
    }

    public void Create_Fast_Dependencies() {
        RepositoryTest.Create_Fast_Dependencies();
        Repository = RepositoryTest.Create_The_Unit();

        ValidatorTest.RepositoryTest = RepositoryTest;
        ValidatorTest.Create_Fast_Dependencies().Crea_The_Unit();
        Validator = ValidatorTest.Unit;
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


    public Validator.FluentValidator.ValidatorTest ValidatorTest = new();
    public RepositoryTest RepositoryTest = new();
}