using Experts.Trader.EditTransaction;
using EntityFramework = Experts.Trader.EditTransaction.Repository.EntityFramework;
using FluentValidator = Experts.Trader.EditTransaction.Validator.FluentValidator;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.EditTransaction;

public class ServiceTest {
    Service CreateUnit() => new(
        dependencies.Validator,
        dependencies.Repository);

    Task<Service.Response> UseTheUnit(Service unit) => unit.Execute(
        arguments.Request,
        arguments.Token);

    Dependencies dependencies = Dependencies.Default();
    Arguments arguments = Arguments.Valid();

    [Fact]
    public async Task Response_Should_NotBeNull() {
        var unit = CreateUnit();
        var response = await UseTheUnit(unit);
        response.Should().NotBeNull();
    }

    [Fact]
    public async Task Response_Request_Should_NotBeNull() {
        var unit = CreateUnit();
        var response = await UseTheUnit(unit);
        response.Request.Should().NotBeNull();
    }

    [Fact]
    public async Task Response_Errors_Should_Reflect_Validation_Issues() {
        var unit = CreateUnit();
        arguments = Arguments.InValid();
        var response = await UseTheUnit(unit);
        response.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Response_Transactions_Should_BeNull_If_There_Is_Validation_Issues() {
        var unit = CreateUnit();
        arguments = Arguments.InValid();
        var response = await UseTheUnit(unit);
        response.Transaction.Should().BeNull();
    }


    [Fact]
    public void AddEditTransaction_ShouldRegisterDependencies() {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationManager();
        configuration.AddInMemoryCollection(new Dictionary<string, string?> {
            { "ConnectionStrings:App", "Data Source=.\\SQLEXPRESS;Initial Catalog=App;User ID=sa;Password=sa!Password;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False" }
        });
        // Act
        services.AddEditTransaction(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var feature = serviceProvider.GetService<Service>();

        feature.Should().NotBeNull();
    }


    public record Dependencies(
        Service.IValidator Validator,
        Service.IRepository Repository) {

        public static Dependencies Default() {
            var validatorTechnology = new FluentValidator.Technology();
            var validatorClient = new FluentValidator.Client(validatorTechnology);
            var validatorAdapter = new FluentValidator.Adapter(validatorClient);


            var entityFramework = DatabaseFactory.Default();
            var repositoryClient = new EntityFramework.Client(entityFramework);
            var repositoryAdapter = new EntityFramework.Adapter(repositoryClient);

            return new Dependencies(
                validatorAdapter,
                repositoryAdapter);
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