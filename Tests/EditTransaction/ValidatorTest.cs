using Common.Validation.Business.Model;
using Experts.Trader.EditTransaction;
using Experts.Trader.EditTransaction.Validator.FluentValidator;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.EditTransaction;

public class ValidatorTest {
    Adapter CreateUnit() => new(
        dependencies.Client);

    Task<List<Error>> UseTheUnit(Adapter unit) => unit.Validate(
        arguments.Request,
        arguments.Token);

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
        var serviceProvider = services.BuildServiceProvider();
        var validatorAdapter = serviceProvider.GetService<Service.IValidator>();
        var validatorClient = serviceProvider.GetService<Adapter.IClient>();
        var validator = serviceProvider.GetService<FluentValidation.IValidator<Service.Request>>();

        validatorAdapter.Should().NotBeNull();
        validatorClient.Should().NotBeNull();
        validator.Should().NotBeNull();
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
            new() { TransactionId = 2, Name = "EUR2" },
            CancellationToken.None);

        public static Arguments InValid() => new(
           new() { TransactionId = -2, Name = "EUR2" },
          CancellationToken.None);
    }
}