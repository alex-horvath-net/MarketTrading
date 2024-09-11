using Experts.Trader.FindTransactions;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EntityFramework = Experts.Trader.FindTransactions.Repository.EntityFramework;
using FluentValidator = Experts.Trader.FindTransactions.Validator.FluentValidator;
using Flag = Experts.Trader.FindTransactions.Flag.Microsoft;
using Clock = Experts.Trader.FindTransactions.Clock.Microsoft;

namespace Tests.FindTransactions;

public class ServiceTest {
    Service CreateUnit() => new(
        dependencies.Validator, 
        dependencies.Flag,
        dependencies.Repository,
        dependencies.Clock);

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
    public async Task Response_Transactions_Should_BeEmpty_If_There_Is_Validation_Issues() {
        var unit = CreateUnit();
        arguments = Arguments.InValid();
        var response = await UseTheUnit(unit);
        response.Transactions.Should().BeEmpty();
    }


    [Fact]
    public void AddRead_ShouldRegisterDependencies() {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationManager();
        configuration.AddInMemoryCollection(new Dictionary<string, string?> {
            { "ConnectionStrings:App", "Data Source=.\\SQLEXPRESS;Initial Catalog=App;User ID=sa;Password=sa!Password;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False" }
        });
        // Act
        services.AddFindTransactions(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var feature = serviceProvider.GetService<Service>();

        feature.Should().NotBeNull();
    }


    public record Dependencies(
        Service.IValidator Validator,
        Service.IFlag Flag,
        Service.IRepository Repository,
        Service.IClock Clock) {

        public static Dependencies Default() {
            var validatorTechnology = new FluentValidator.Technology();
            var validatorClient = new FluentValidator.Client(validatorTechnology);
            var validatorAdapter = new FluentValidator.Adapter(validatorClient);

            var flagClient = new Flag.Client();
            var flagAdapter = new Flag.Adapter(flagClient);

            var dbFactory = new DatabaseFactory();
            var entityFramework = dbFactory.Default();
            var repositoryClient = new EntityFramework.Client(entityFramework);
            var repositoryAdapter = new EntityFramework.Adapter(repositoryClient);

            var clockClient = new Clock.Client();
            var clockAdapter = new Clock.Adapter(clockClient);

            return new Dependencies(
                validatorAdapter,
                flagAdapter,
                repositoryAdapter,
                clockAdapter);
        }
    }

    public record Arguments(Service.Request Request, CancellationToken Token) {
        public static Arguments Valid() => new(
            new() { UserId = "alad", Name = "USD" },
            CancellationToken.None);

        public static Arguments InValid() => new(
          new() { Name = "US" },
          CancellationToken.None);
    }
}