using Common.Valdation.Adapters.Fluentvalidation;
using Common.Valdation.Business;
using Common.Valdation.Technology.FluentValidation;
using Experts.Trader.FindTransactions;
using Experts.Trader.FindTransactions.Read.Adapters;
using Experts.Trader.FindTransactions.Read.Business;
using Experts.Trader.FindTransactions.Read.Technology;
using Experts.Trader.FindTransactions.Validate.Technology;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.FindTransactions;

public class FeatureTest {
    Service CreateUnit() => new(dependencies.Validator, dependencies.Repository);
    Task<Response> UseTheUnit(Service unit) => unit.Execute(arguments.Request, arguments.Token);
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
        IValidatorAdapter<Request> Validator,
        IRepositoryAdapter Repository) {

        public static Dependencies Default() {
            var validator = new Validator();
            var validatorClient = new ValidatorClient<Request>(validator);
            var validatorAdapter = new ValidatorAdapter<Request>(validatorClient);

            var dbFactory = new DatabaseFactory();
            var db = dbFactory.Default();
            var repositoryClient = new RepositoryClient(db);
            var repositoryAdapter = new RepositoryAdapter(repositoryClient);

            return new Dependencies(validatorAdapter, repositoryAdapter);
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