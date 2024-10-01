using Experts.Trader.FindTransactions;

namespace Tests.Trader.Find_Transactions;

public class Service_Test {

    public Service.IValidator Validator;
    public Service.IFlag Flag;
    public Service.IRepository Repository;
    public Service.IClock Clock;
    public Service Unit;
    public void Create_Unit() => Unit = new Service(Validator, Flag, Repository, Clock);


    public Response Response;
    public Request Request;
    public CancellationToken Token;
    public async Task Use_The_Unit() => Response = await Unit.Execute(Request, Token);

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

    [IntegrationFact]
    public void Use_DI() {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationManager();
        configuration.AddInMemoryCollection(new Dictionary<string, string?> { { "ConnectionStrings:App", "" } });
        // Act
        services.AddService(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var feature = serviceProvider.GetService<IService>();

        feature.Should().NotBeNull();
    }


    public void Create_Fast_Dependencies() {
        Repository = RepositoryTest.With_Fast_Dependencies().Create_Unit();

        ValidatorTest.Create_Default_Dependencies().Create_The_Unit();
        Validator = ValidatorTest.Unit;

        var flagClient = new Flag.Client();
        Flag = new Flag(flagClient);

        var clockClient = new Clock.Client();
        Clock = new Clock(clockClient);
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


    public Validator_Test ValidatorTest = new();
    public Repository_Client_Want_To RepositoryTest = new();
}

