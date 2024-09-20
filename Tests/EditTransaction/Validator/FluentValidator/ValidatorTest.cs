using Common.Validation.Business.Model;
using Experts.Trader.EditTransaction;
using Experts.Trader.EditTransaction.Validator.FluentValidator;
using EntityFramework = Experts.Trader.EditTransaction.Repository.EntityFramework;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.EditTransaction.Validator.FluentValidator;

public class ValidatorTest {
    public Service.IValidator CreateUnit() => new Adapter(Client);
    public Task<List<Error>> Run(Service.IValidator unit) => unit.Validate(Request, Token);

    [Fact]
    public async Task It_Should_Reviel_No_Errors_Of_Valid_Request() {
        var dependecies = CreateFastDependencies();
        var unit = CreateUnit();
        var arguments = CreateValidRequest();
        var response = await Run(unit);
        response.Should().BeEmpty();
    }

    [Fact]
    public async Task It_Should_Reviel_Errors_Of_Non_Valid_Request() {
        var dependecies = CreateFastDependencies();
        var unit = CreateUnit();
        var arguments = CreateInValidRequest();
        var response = await Run(unit);
        response.Should().NotBeEmpty();
    }

    [Fact]
    public void It_Is_DI_Ready() {
        // Arrange
        var services = new ServiceCollection();
        // Act
        services.AddValidatorAdapter();

        // Assert
        var sp = services.BuildServiceProvider();
        var adapter = sp.GetService<Service.IValidator>();
        var client = sp.GetService<Adapter.IClient>();
        var technology = sp.GetService<FluentValidation.IValidator<Service.Request>>();

        adapter.Should().NotBeNull();
        client.Should().NotBeNull();
        technology.Should().NotBeNull();
    }

    public Adapter.IClient Client;
    public EntityFramework.Adapter.IClient RepositoryClient;

    public Service.Request Request;
    public CancellationToken Token;

    public (Adapter.IClient Client, EntityFramework.Adapter.IClient RepositoryClient) CreateFastDependencies() {

        if (RepositoryTest == null) {
            RepositoryTest = new Repository.EntityFramework.RepositoryTest();
            RepositoryTest.CreateFastDependencies();
        }

        RepositoryClient = RepositoryTest.Client;

        var technology = new Technology(RepositoryClient);
        Client = new Client(technology);
        return (Client, RepositoryClient);
    }

    public (Service.Request, CancellationToken) CreateValidRequest() {
        Request = new() { UserId = "aladar", TransactionId = 2, Name = "USD_NEW" };
        Token = CancellationToken.None;
        return (Request, Token);
    }

    public (Service.Request, CancellationToken) CreateInValidRequest() {
        Request = new() { UserId = "aladar", TransactionId = -2, Name = "US" };
        Token = CancellationToken.None;
        return (Request,Token);
    }

    public Repository.EntityFramework.RepositoryTest RepositoryTest;
}
