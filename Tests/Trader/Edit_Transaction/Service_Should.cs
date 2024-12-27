using Common.Technology;
using DomainExperts.Trader.EditTransaction;

namespace Tests.Trader.Edit_Transaction;

public class Service_Should {
    public BusinessNeed.IValidator Validator;
    public BusinessNeed.IRepository Repository;
    public BusinessNeed Unit;
    public void Crea_The_Unit() => Unit = new BusinessNeed(Validator, Repository);

    public BusinessNeed.Response Response;
    public BusinessNeed.Request Request;
    public CancellationToken Token;
    public async Task Use_The_Unit() => Response = await Unit.Execute(Request, Token);


    [Xunit.Fact]
    public async Task Present_Response() {
        Create_Fast_Dependencies();
        Crea_The_Unit();
        Create_Valid_Arguments();
        await Use_The_Unit();
        Response.Should().NotBeNull(); 
    }

    [Xunit.Fact]
    public async Task Present_Request() {
        Create_Fast_Dependencies();
        Crea_The_Unit();
        Create_Valid_Arguments();
        await Use_The_Unit();
        Response.Request.Should().NotBeNull();
    }

    [Xunit.Fact]
    public async Task Present_No_Errors_For_Valid_Request() {
        Create_Fast_Dependencies();
        Crea_The_Unit();
        Create_Valid_Arguments();
        await Use_The_Unit();
        Response.Errors.Should().BeEmpty();
    }

    [Xunit.Fact]
    public async Task Present_Transactions_For_Valid_Request() {
        Create_Fast_Dependencies();
        Crea_The_Unit();
        Create_Valid_Arguments();
        await Use_The_Unit();
        Response.Transaction.Should().NotBeNull();
    }

    [Xunit.Fact]
    public async Task Present_Errors_For_Non_Valid_Request() {
        Create_Fast_Dependencies(); 
        Crea_The_Unit();
        Create_Non_Valid_Arguments();
        await Use_The_Unit();
        Response.Errors.Should().NotBeEmpty();
    }

    [Xunit.Fact]
    public async Task Present_No_Transactions_For_Non_Valid_Request() {
        Create_Fast_Dependencies();
        Crea_The_Unit(); 
        Create_Non_Valid_Arguments();
        await Use_The_Unit();
        Response.Transaction.Should().BeNull();
    }


    [IntegrationFactAttribute]
    public void Use_DI() {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationManager();
        configuration.AddInMemoryCollection(new Dictionary<string, string?> {
            { "ConnectionStrings:App", "" },
            { "ConnectionStrings:Identity", "" }
        });
        // Act
        services.AddCommonTechnology(configuration);
        services.AddEditTransaction(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var feature = serviceProvider.GetService<BusinessNeed>();

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