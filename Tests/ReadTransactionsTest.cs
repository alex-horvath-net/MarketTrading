using Common.Business;
using Experts.Trader.ReadTransactions;
using Experts.Trader.ReadTransactions.Read;
using Experts.Trader.ReadTransactions.Validate;
using FluentAssertions;

namespace Tests;

public class ReadTransactionsTest {
    Feature CreateUnit() => new(
        dependencies.Validator,
        dependencies.Repository);
    Task<Response> UseTheUnit(Feature unit) => unit.Execute(arguments.Request, arguments.Token);
    Dependencies dependencies = Dependencies.Default();
    Arguments arguments = Arguments.Valid();

    [Fact]
    public async Task response_Should_NotBeNull() {
        var unit = CreateUnit();
        var response = await UseTheUnit(unit);
        response.Should().NotBeNull();
    }

    [Fact]
    public async Task response_Request_Should_NotBeNull() {
        var unit = CreateUnit();
        var response = await UseTheUnit(unit);
        response.Request.Should().NotBeNull();
    }

    [Fact]
    public async Task response_Errors_Should_NotBeNull() {
        var unit = CreateUnit();
        var response = await UseTheUnit(unit);
        response.Errors.Should().NotBeNull();
    }

    [Fact]
    public async Task response_Errors_Should_BeEmpty_If_Request_Valid() {
        var unit = CreateUnit();
        var response = await UseTheUnit(unit);
        response.Errors.Should().BeEmpty();
    }

    [Fact]
    public async Task response_Errors_Should_BeEmpty_If_Request_Is_Not_Valid() {
        var unit = CreateUnit();
        arguments = Arguments.InValid();
        var response = await UseTheUnit(unit);
        response.Errors.Should().NotBeEmpty();
    }
     

    [Fact]
    public async Task response_Transactions_Should_NotBeNull() {
        var unit = CreateUnit();
        var response = await UseTheUnit(unit);
        response.Transactions.Should().NotBeNull();
    }

    [Fact]
    public async Task response_Transactions_Should_BeOfType_List_Transaction() {
        var unit = CreateUnit();
        var response = await UseTheUnit(unit);
        response.Transactions.Should().BeOfType<List<Transaction>>();
    }

    [Fact]
    public async Task response_Transactions_Should_NotBeEmpty() {
        var unit = CreateUnit();
        var response = await UseTheUnit(unit);
        response.Transactions.Should().NotBeEmpty();
    }

    //[Fact]
    //public async Task dependencies_Repository_Should_Received_1_Read() {
    //    var unit = CreateUnit();
    //    var response = await UseTheUnit(unit);
    //    dependencies.Repository.Received(1).Read(default); 
    //}


    public record Dependencies(
        Feature.IValidatorAdapterPort Validator,
        Feature.IRepositoryAdapterPort Repository) {

        public static Dependencies Default() {
            //var repository = Substitute.For<Feature.IRepository>();
            //repository.Read(default).Returns([]);
            var fluentValidator = new ValidatorTechnologyPlugin.RequestValidator();
            var validatorTechnologyPlugin = new ValidatorTechnologyPlugin(fluentValidator);
            var validatorAdapterPlugin = new ValidatorAdapterPlugin(validatorTechnologyPlugin);

            var entityFramework = DatabaseFactory.Default();
            var repositoryTechnologyPlugin = new RepositoryTechnologyPlugin(entityFramework);
            var repositoryAdapterPlugin = new RepositoryAdapterPlugin(repositoryTechnologyPlugin);

            return new Dependencies(validatorAdapterPlugin, repositoryAdapterPlugin);
        }
    }

    public record Arguments(Request Request, CancellationToken Token) {
        public static Arguments Valid() => new(
            new() { Name = "USD" },
            CancellationToken.None);

        public static Arguments InValid() => new(
          new() { Name = null },
          CancellationToken.None);
    }
}