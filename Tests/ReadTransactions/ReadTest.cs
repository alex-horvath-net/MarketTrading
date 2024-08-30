using Common.Business;
using Experts.Trader.ReadTransactions;
using FluentAssertions;

namespace Tests.ReadTransactions;

public class ReadTest {
    RepositoryAdapterPlugin CreateUnit() => new(dependencies.RepositoryTechnologyPlugin);
    Task<List<Transaction>> UseTheUnit(RepositoryAdapterPlugin unit) => unit.ReadTransaction(arguments.Request, arguments.Token);
    Dependencies dependencies = Dependencies.Default();
    Arguments arguments = Arguments.Valid();

    
    [Fact]
    public async Task It_Should_Find_Transactions() {
        var unit = CreateUnit();
        var transactions = await UseTheUnit(unit);
        transactions.Should().NotBeNull();
    }

    [Fact]
    public async Task It_Should_Find_List_Of_Transaction() {
        var unit = CreateUnit();
        var transactions = await UseTheUnit(unit);
        transactions.Should().BeOfType<List<Transaction>>();
    }

    [Fact]
    public async Task It_Should_Find_Matching_Transactions() {
        var unit = CreateUnit();
        var transactions = await UseTheUnit(unit);
        transactions.Should().NotBeEmpty();
    }

    [Fact]
    public async Task It_Should_Not_Find_Non_Matching_Transactions() {
        var unit = CreateUnit();
        arguments = Arguments.InValid();
        var transactions = await UseTheUnit(unit);
        transactions.Should().BeEmpty();
    }



    public record Dependencies(
        RepositoryTechnologyPlugin RepositoryTechnologyPlugin) {

        public static Dependencies Default() {
            var databaseFactory = new DatabaseFactory();
            var entityFramework = databaseFactory.Default();
            var repositoryTechnologyPlugin = new RepositoryTechnologyPlugin(entityFramework);

            return new Dependencies(repositoryTechnologyPlugin);
        }
    }

    public record Arguments(Feature.Request Request, CancellationToken Token) {
        public static Arguments Valid() => new(
            new() { Name = "USD" },
            CancellationToken.None);

        public static Arguments InValid() => new(
          new() { Name = "USD_Typo" },
          CancellationToken.None);
    }
}