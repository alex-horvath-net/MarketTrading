using Common.Validation.Business.Model;
using Experts.Trader.EditTransaction;
using Experts.Trader.EditTransaction.Repository.EntityFramework;
using Experts.Trader.EditTransaction.Validator.FluentValidator;

namespace Tests.Trader.Edit_Transaction;

public class Validation_Should {
    public Experts.Trader.EditTransaction.Validator.FluentValidator.Adapter.IClient Client;
    public Experts.Trader.EditTransaction.Repository.EntityFramework.Adapter.IClient RepositoryClient;
    public Service.IValidator Unit;
    public void Crea_The_Unit() => Unit = new Experts.Trader.EditTransaction.Validator.FluentValidator.Adapter(Client);

    public List<Error> Response;
    public Service.Request Request;
    public CancellationToken Token;
    public async Task Use_The_Unit() => Response = await Unit.Validate(Request, Token);

    [Fact]
    public async Task Provide_No_Errors_For_Valid_Request() {
        Create_Fast_Dependencies();
        Crea_The_Unit();
        Create_Valid_Arguments();
        await Use_The_Unit();
        Response.Should().BeEmpty();
    }

    [Fact]
    public async Task Provide_Errors_For_Non_Valid_Request() {
        Create_Fast_Dependencies();
        Crea_The_Unit();
        Create_Non_Valid_Arguments();
        await Use_The_Unit();
        Response.Should().NotBeEmpty();
    }

    [IntegrationFact]
    public void Use_DI() {
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

    public Validation_Should Create_Fast_Dependencies() {

        if (RepositoryTest == null) {
            RepositoryTest = new Repository_Should();
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

    public Repository_Should RepositoryTest;
}
