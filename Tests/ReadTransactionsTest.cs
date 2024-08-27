using Common.Business;
using FluentAssertions;

namespace Experts.Trader.ReadTransactions.Business;

public class ReadTransactionsTest {
    Feature CreateUnit() => new(dependencies.Repository);
    Task<Response> UseTheUnit(Business.Feature unit) => unit.Execute(arguments.Request, arguments.Token);
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


    public record Dependencies(AdapterPorts.IRepositoryAdapterPort Repository) {
        public static Dependencies Default() {
            //var repository = Substitute.For<Feature.IRepository>();
            //repository.Read(default).Returns([]);
            var db = DatabaseFactory.Default();
            var pluginDb = new Technology.RepositoryTechnologyPlugin(db);
            var repository = new Adapters.RepositoryAdapterPlugin(pluginDb);
            return new Dependencies(repository);
        }
    }

    public record Arguments(Request Request, CancellationToken Token) {
        public static Arguments Default() => new(new(), CancellationToken.None);
    }
}