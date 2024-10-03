using Common.Validation.Business.Model;
using Experts.Trader.FindTransactions;
using Experts.Trader.FindTransactions.WorkSteps;

namespace Tests.Trader.Find_Transactions;

public class Validator_Test {

    public Validator.IClient? Client;
    public Service.IValidator? Unit;
    public void Create_The_Unit() => Unit = new Validator(Client);

    public Request? Request;
    public CancellationToken Token;
    public List<Error>? Response;
    public async Task Use_The_Unit() => Response = await Unit.Validate(Request, Token);

    [Xunit.Fact]
    public async Task Detect_Errors() {
        Create_Default_Dependencies();
        Create_The_Unit();
        Create_Valid_Arguments();
        await Use_The_Unit();
        Response.Should().BeOfType<List<Error>>();
    }

    [Xunit.Fact]
    public async Task It_Can_Not_Find_Errors_In_Valid_Request() {
        Create_Default_Dependencies();
        Create_The_Unit();
        Create_Valid_Arguments();
        await Use_The_Unit();
        Response.Should().BeEmpty();
    }



    [Xunit.Fact]
    public async Task It_Should_Reviel_Errors_Of_Non_Valid_Request() {
        Create_Default_Dependencies();
        Create_The_Unit();
        Create_Non_Valid_Arguments();
        await Use_The_Unit();
        Response.Should().NotBeEmpty();
    }

    [IntegrationFactAttribute]
    public void Use_DI() {
        // Arrange
        var services = new ServiceCollection();
        // Act
        services.AddValidator();

        // Assert
        var sp = services.BuildServiceProvider();
        var adapter = sp.GetService<Service.IValidator>();
        var client = sp.GetService<Validator.IClient>();
        var technology = sp.GetService<FluentValidation.IValidator<Request>>();

        adapter.Should().NotBeNull();
        client.Should().NotBeNull();
        technology.Should().NotBeNull();
    }

    public Validator_Test Create_Default_Dependencies() {
        var technology = new Validator.Client.Technology();
        Client = new Validator.Client(technology);
        return this;
    }

    public void Create_Valid_Arguments() {
        Request = new() { UserId = "aladar", Name = "USD" };
        Token = CancellationToken.None;
    }

    public void Create_Non_Valid_Arguments() {
        Request = new() { UserId = "aladar", Name = "US" };
        Token = CancellationToken.None;
    }
}
