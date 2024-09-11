using Common.Validation.Business.Model;
using Experts.Trader.FindTransactions;
using Experts.Trader.FindTransactions.Validator.FluentValidator;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.FindTransactions;

public class ValidatorTest {
    Adapter CreateUnit() => new(dependencies.Client);
    Task<List<Error>> UseTheUnit(Adapter unit) => unit.Validate(arguments.Request, arguments.Token);
    Dependencies dependencies = Dependencies.Default();
    Arguments arguments = Arguments.Valid();


    [Fact]
    public async Task It_Should_Reviel_Errors() {
        var unit = CreateUnit();
        var errors = await UseTheUnit(unit);
        errors.Should().NotBeNull();
    }

    [Fact]
    public async Task It_Should_Reviel_No_Errors_Of_Valid_Request() {
        var unit = CreateUnit();
        var errors = await UseTheUnit(unit);
        errors.Should().BeEmpty();
    }

    [Fact]
    public async Task It_Should_Reviel_Errors_Of_Non_Valid_Request() {
        var unit = CreateUnit();
        arguments = Arguments.InValid();
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

    public record Dependencies(Adapter.IClient Client) {

        public static Dependencies Default() {
            var technology = new Technology();
            var client = new Client(technology);
            return new Dependencies(client);
        }
    }

    public record Arguments(Service.Request Request, CancellationToken Token) {
        public static Arguments Valid() => new(
            new() { Name = "USD" },
            CancellationToken.None);

        public static Arguments InValid() => new(
          new() { Name = "US" },
          CancellationToken.None);
    }
}