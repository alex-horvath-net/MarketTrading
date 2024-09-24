using Experts.Trader.EditTransaction;

namespace Tests.Trader.Edit_Transaction;

public class Service_Should {
    public Service.IValidator Validator;
    public Service.IRepository Repository;
    public Service Unit;
    public void Crea_The_Unit() => Unit = new Service(Validator, Repository);

    public Service.Response Response;
    public Service.Request Request;
    public CancellationToken Token;
    public async Task Use_The_Unit() => Response = await Unit.Execute(Request, Token);


    [Fact]
    public async Task Present_Response() {
        Create_Fast_Dependencies();
        Crea_The_Unit();
        Create_Valid_Arguments();
        await Use_The_Unit();
        Response.Should().NotBeNull(); 
    }

    [Fact]
    public async Task Present_Request() {
        Create_Fast_Dependencies();
        Crea_The_Unit();
        Create_Valid_Arguments();
        await Use_The_Unit();
        Response.Request.Should().NotBeNull();
    }

    [Fact]
    public async Task Present_No_Errors_For_Valid_Request() {
        Create_Fast_Dependencies();
        Crea_The_Unit();
        Create_Valid_Arguments();
        await Use_The_Unit();
        Response.Errors.Should().BeEmpty();
    }

    [Fact]
    public async Task Present_Transactions_For_Valid_Request() {
        Create_Fast_Dependencies();
        Crea_The_Unit();
        Create_Valid_Arguments();
        await Use_The_Unit();
        Response.Transaction.Should().NotBeNull();
    }

    [Fact]
    public async Task Present_Errors_For_Non_Valid_Request() {
        Create_Fast_Dependencies(); 
        Crea_The_Unit();
        Create_Non_Valid_Arguments();
        await Use_The_Unit();
        Response.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Present_No_Transactions_For_Non_Valid_Request() {
        Create_Fast_Dependencies();
        Crea_The_Unit(); 
        Create_Non_Valid_Arguments();
        await Use_The_Unit();
        Response.Transaction.Should().BeNull();
    }


    [IntegrationFact]
    public void Use_DI() {
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


    public Validation_Should ValidatorTest = new();
    public Repository_Should RepositoryTest = new();
}