using Common.Validation.Business.Model;
using Experts.Trader.EditTransaction;
using Experts.Trader.EditTransaction.Repository.EntityFramework;
using Experts.Trader.EditTransaction.Validator.FluentValidator;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tests.EditTransaction.Repository_EntityFramework;

namespace Tests.EditTransaction.Validator.FluentValidator;

public class ValidatorTest {
    public Experts.Trader.EditTransaction.Validator.FluentValidator.Adapter.IClient Client;
    public Experts.Trader.EditTransaction.Repository.EntityFramework.Adapter.IClient RepositoryClient;
    public Service.IValidator Unit;
    public void Crea_The_Unit() => Unit = new Experts.Trader.EditTransaction.Validator.FluentValidator.Adapter(Client);

    public List<Error> Response;
    public Service.Request Request;
    public CancellationToken Token;
    public async Task Use_The_Unit() => Response = await Unit.Validate(Request, Token);

    [Fact]
    public async Task Valid_Request_Should_Cause_No_Errors() {
        Create_Fast_Dependencies();
        Crea_The_Unit();
        Create_Valid_Arguments();
        await Use_The_Unit();
        Response.Should().BeEmpty();
    }

    [Fact]
    public async Task It_Should_Reviel_Errors_Of_Non_Valid_Request() {
        Create_Fast_Dependencies();
        Crea_The_Unit();
        Create_Non_Valid_Arguments();
        await Use_The_Unit();
        Response.Should().NotBeEmpty();
    }

    [Fact]
    public void It_Is_DI_Ready() {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationManager();
        // Act
        services.AddRepositoryAdapter(configuration);
        services.AddValidatorAdapter();

        // Assert
        var sp = services.BuildServiceProvider();
        var adapter = sp.GetService<Service.IValidator>();
        var client = sp.GetService<Experts.Trader.EditTransaction.Validator.FluentValidator.Adapter.IClient>();
        var technology = sp.GetService<FluentValidation.IValidator<Service.Request>>();

        adapter.Should().NotBeNull();
        client.Should().NotBeNull();
        technology.Should().NotBeNull();
    }

    public ValidatorTest Create_Fast_Dependencies() {

        if (RepositoryTest == null) {
            RepositoryTest = new RepositoryTest();
            RepositoryTest.Create_Fast_Dependencies();
        }

        RepositoryClient = RepositoryTest.Client;

        var technology = new Technology(RepositoryClient);
        Client = new Experts.Trader.EditTransaction.Validator.FluentValidator.Client(technology);
        return this;
    }

    public void Create_Valid_Arguments() {
        Request = new() { UserId = "aladar", TransactionId = 2, Name = "USD_NEW" };
        Token = CancellationToken.None;
    }

    public void Create_Non_Valid_Arguments() {
        Request = new() { UserId = "aladar", TransactionId = -2, Name = "US" };
        Token = CancellationToken.None;
    }

    public RepositoryTest RepositoryTest;
}
