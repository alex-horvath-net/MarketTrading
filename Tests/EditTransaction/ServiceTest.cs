using Experts.Trader.EditTransaction;
using EntityFramework = Experts.Trader.EditTransaction.Repository.EntityFramework;
using FluentValidator = Experts.Trader.EditTransaction.Validator.FluentValidator;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.EditTransaction;

public class ServiceTest {
    Service CreateUnit() => new(Validator, Repository);
    Task<Service.Response> Run(Service unit) => unit.Execute(Request, Token);


    [Fact]
    public async Task Response_Should_NotBeNull() {
        var dependecies = CreateFastDependencies();
        var unit = CreateUnit();
        var arguments = CreateValidRequest();
        var response = await Run(unit);
        response.Should().NotBeNull();
    }

    [Fact]
    public async Task Response_Request_Should_NotBeNull() {
        var dependecies = CreateFastDependencies();
        var unit = CreateUnit();
        var arguments = CreateValidRequest();
        var response = await Run(unit);
        response.Request.Should().NotBeNull();
    }

    [Fact]
    public async Task Response_Errors_Should_Reflect_Validation_Issues() {
        var dependecies = CreateFastDependencies();
        var unit = CreateUnit();
        var arguments = CreateInValidRequest();
        var response = await Run(unit);
        response.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Response_Transactions_Should_BeNull_If_There_Is_Validation_Issues() {
        var dependecies = CreateFastDependencies();
        var unit = CreateUnit();
        var arguments = CreateInValidRequest();
        var response = await Run(unit);
        response.Transaction.Should().BeNull();
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

    public Service.IValidator Validator;
    public Service.IRepository Repository;

    public Service.Request Request;
    public CancellationToken Token;

    public (Service.IValidator Validator, Service.IRepository Repository) CreateFastDependencies() {
        RepositoryTest.CreateFastDependencies();
        Repository = RepositoryTest.CreateUnit();

        ValidatorTest = new Validator.FluentValidator.ValidatorTest();
        ValidatorTest.RepositoryTest = RepositoryTest;
        ValidatorTest.CreateFastDependencies();
        Validator = ValidatorTest.CreateUnit();

        return (Validator, Repository);
    }

    public (Service.Request, CancellationToken) CreateValidRequest() =>
        ValidatorTest.CreateValidRequest();

    public (Service.Request, CancellationToken) CreateInValidRequest() =>
        ValidatorTest.CreateInValidRequest();


    public Validator.FluentValidator.ValidatorTest ValidatorTest = new();
    public Repository.EntityFramework.RepositoryTest RepositoryTest = new();
}