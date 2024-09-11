using Common.Valdation.Technology.FluentValidation;
using Common.Validation.Business.Model;
using Common.Validation.FluentValidator.Adapters;
using Experts.Trader.FindTransactions;
using Experts.Trader.FindTransactions.Validate;
using Experts.Trader.FindTransactions.Validate.Technology;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.FindTransactions;

public class ValidationTest {
    CommonAdapter<Request> CreateUnit() => new(dependencies.ValidatorClient);
    Task<List<Error>> UseTheUnit(CommonAdapter<Request> unit) => unit.Validate(arguments.Request, arguments.Token);
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
        var validatorAdapter = serviceProvider.GetService<Common.Validation.Business.IValidator<Request>>();
        var validatorClient = serviceProvider.GetService<ICommonClient<Request>>();
        var validator = serviceProvider.GetService<FluentValidation.IValidator<Request>>();

        validatorAdapter.Should().NotBeNull();
        validatorClient.Should().NotBeNull();
        validator.Should().NotBeNull();
    }

    public record Dependencies(ICommonClient<Request> ValidatorClient) {

        public static Dependencies Default() {
            var validator = new Validator();
            var validatiorClient = new ValidatorClient<Request>(validator);
            return new Dependencies(validatiorClient);
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