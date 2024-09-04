using Experts.Trader.ReadTransactions;
using Experts.Trader.ReadTransactions.Business.Logic;
using Experts.Trader.ReadTransactions.Validate;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.ReadTransactions;

public class ValidationTest {
    Adapter CreateUnit() => new(dependencies.ValidatorTechnology);
    Task<List<string>> UseTheUnit(Adapter unit) => unit.Validate(arguments.Request, arguments.Token);
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
        services.AddValidation();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var validatorAdapterPlugun = serviceProvider.GetService<IValidatort>();
        var validatorTechnologyPlugin = serviceProvider.GetService<Adapter.IValidatorTechnologyPort>();
        var fluentValidator = serviceProvider.GetService<IValidator<Request>> ();

        validatorAdapterPlugun.Should().NotBeNull();
        validatorTechnologyPlugin.Should().NotBeNull();
        fluentValidator.Should().NotBeNull();
    }

    public record Dependencies(
    Adapter.IValidatorTechnologyPort ValidatorTechnology) {

        public static Dependencies Default() {
            var fluentValidator = new Validator.RequestValidator();
            var validatorTechnologyPlugin = new Validator(fluentValidator);
            return new Dependencies(validatorTechnologyPlugin);
        }
    }

    public record Arguments(Request Request, CancellationToken Token) {
        public static Arguments Valid() => new(
            new() { Name = "USD" },
            CancellationToken.None);

        public static Arguments InValid() => new(
          new() { Name = "US" },
          CancellationToken.None);
    }
}