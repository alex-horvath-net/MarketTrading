using Common.Valdation.Adapters.Fluentvalidation;
using Common.Valdation.Business;
using Common.Valdation.Business.Model;
using Common.Valdation.Technology.FluentValidation;
using Experts.Trader.EditTransaction;
using Experts.Trader.EditTransaction.Validation;
using Experts.Trader.EditTransaction.Validation.Technology;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.EditTransaction;

public class ValidationTest {
    ValidatorAdapter<Request> CreateUnit() => new(dependencies.ValidatorClient);
    Task<List<Error>> UseTheUnit(ValidatorAdapter<Request> unit) => unit.Validate(arguments.Request, arguments.Token);
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
        var validatorAdapter = serviceProvider.GetService<IValidatorAdapter<Request>>();
        var validatorClient = serviceProvider.GetService<IValidatorClient<Request>>();
        var validator = serviceProvider.GetService<IValidator<Request>>();

        validatorAdapter.Should().NotBeNull();
        validatorClient.Should().NotBeNull();
        validator.Should().NotBeNull();
    }

    public record Dependencies(IValidatorClient<Request> ValidatorClient) {

        public static Dependencies Default() {
            var validator = new Validator();
            var validatiorClient = new ValidatorClient<Request>(validator);
            return new Dependencies(validatiorClient);
        }
    }

    public record Arguments(Request Request, CancellationToken Token) {
        public static Arguments Valid() => new(
            new() { Id = 2, Name = "EUR2" },
            CancellationToken.None);

        public static Arguments InValid() => new(
           new() { Id = -2, Name = "EUR2" },
          CancellationToken.None);
    }
}