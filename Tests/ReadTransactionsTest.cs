using Businsess;
using FluentAssertions;
using Trader.Transactions.ReadTransactions;

namespace Tests;


public class ReadTransactionsTest {
    Business CreateUnit() => new(dependencies.Repository);
    Task<Business.Response> UseTheUnit(Business unit) => unit.Execute(arguments.Request, arguments.Token);
    readonly Dependencies dependencies = Dependencies.Default();
    readonly Arguments arguments = Arguments.Default();

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


    public record Dependencies(Business.IRepository Repository) {
        public static Dependencies Default() {
            //var repository = Substitute.For<Feature.IRepository>();
            //repository.Read(default).Returns([]);
            var db = DatabaseFactory.Default();
            var pluginDb = new Trader.Transactions.ReadTransactions.Technology.Repository(db);
            var repository = new Trader.Transactions.ReadTransactions.Adapters.Repository(pluginDb);
            return new Dependencies(repository);
        }
    }

    public record Arguments(Business.Request Request, CancellationToken Token) {
        public static Arguments Default() => new(new(), CancellationToken.None);
    }
}