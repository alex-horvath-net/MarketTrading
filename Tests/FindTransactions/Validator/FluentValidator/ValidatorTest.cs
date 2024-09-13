using Common.Validation.Business.Model;
using Experts.Trader.FindTransactions;
using Experts.Trader.FindTransactions.Validator.FluentValidator;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.FindTransactions.Validator.FluentValidator;

public class ValidatorTest : Driver {

    [Fact]
    public async Task It_Should_Reviel_Errors() {
        DefaultDependencies();
        var unit = CreateUnit();
        ValidArguments();
        var errors = await UseTheUnit(unit);
        errors.Should().NotBeNull();
    }

    [Fact]
    public async Task It_Should_Reviel_No_Errors_Of_Valid_Request() {
        DefaultDependencies();
        var unit = CreateUnit();
        ValidArguments();
        var errors = await UseTheUnit(unit);
        errors.Should().BeEmpty();
    }

    [Fact]
    public async Task It_Should_Reviel_Errors_Of_Non_Valid_Request() {
        DefaultDependencies();
        var unit = CreateUnit();
        InValidArguments();
        var errors = await UseTheUnit(unit);
        errors.Should().NotBeEmpty();
    }

    [Fact]
    public void AddValidation_ShouldRegisterDependencies() {
        // Arrange
        var services = new ServiceCollection();
        // Act
        services.AddValidator();

        // Assert
        var sp = services.BuildServiceProvider();
        var adapter = sp.GetService<Service.IValidator>();
        var client = sp.GetService<Adapter.IClient>();
        var technology = sp.GetService<FluentValidation.IValidator<Service.Request>>();

        adapter.Should().NotBeNull();
        client.Should().NotBeNull();
        technology.Should().NotBeNull();
    }
}

public class Driver {

    public Adapter CreateUnit() => new(Client);

    public Task<List<Error>> UseTheUnit(Adapter unit) => unit.Validate(Request, Token);

    public Adapter.IClient Client;

    public Service.Request Request;
    public CancellationToken Token;

    public void DefaultDependencies() {
        var technology = new Technology();
        Client = new Client(technology);
    }

    public void ValidArguments() {
        Request = new() { UserId = "aladar", Name = "USD" };
        Token = CancellationToken.None;
    }

    public void InValidArguments() {
        Request = new() { UserId = "aladar", Name = "US" };
        Token = CancellationToken.None;
    }
}