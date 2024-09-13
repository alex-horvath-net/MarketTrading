using Common.Validation.Business.Model;
using Experts.Trader.FindTransactions;
using Experts.Trader.FindTransactions.Validator.FluentValidator;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.FindTransactions.Validator.FluentValidator;

public class ValidatorTest : Driver {
    Adapter CreateUnit() => new(Client);
    Task<List<Error>> UseTheUnit(Adapter unit) => unit.Validate(Request, Token);

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
}
