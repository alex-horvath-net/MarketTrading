using Common.Validation.Business.Model;
using Experts.Trader.EditTransaction;

namespace Tests.Trader.Edit_Transaction;

public class Validation_Should {
    public Validator.IClient Client;
    public Repository.IClient RepositoryClient;
    public Service.IValidator Unit;
    public void Crea_The_Unit() => Unit = new Validator(Client);

    public List<Error> Response;
    public Service.Request Request;
    public CancellationToken Token;
    public async Task Use_The_Unit() => Response = await Unit.Validate(Request, Token);

    [Xunit.Fact]
    public async Task Provide_No_Errors_For_Valid_Request() {
        Create_Fast_Dependencies();
        Crea_The_Unit();
        Create_Valid_Arguments();
        await Use_The_Unit();
        Response.Should().BeEmpty();
    }

    [Xunit.Fact]
    public async Task Provide_Errors_For_Non_Valid_Request() {
        Create_Fast_Dependencies();
        Crea_The_Unit();
        Create_Non_Valid_Arguments();
        await Use_The_Unit();
        Response.Should().NotBeEmpty();
    }

    [IntegrationFactAttribute]
    public void Use_DI() {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationManager();
        configuration.AddInMemoryCollection(new Dictionary<string, string?> { { "ConnectionStrings:App", "" } });
        // Act
        services.AddRepositoryAdapter(configuration);
        services.AddValidatorAdapter();

        // Assert
        var sp = services.BuildServiceProvider();
        var adapter = sp.GetService<Service.IValidator>();
        var client = sp.GetService<Validator.IClient>();
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

        var technology = new Validator.Client.Technology(RepositoryClient);
        Client = new Validator.Client(technology);
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
