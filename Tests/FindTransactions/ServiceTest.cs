using Experts.Trader.FindTransactions;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.FindTransactions;

public class ServiceTest : Driver {
    Service CreateTheUnit() => new(Validator, Flag, Repository, Clock);
    Task<Service.Response> UseTheUnit(Service unit) => unit.Execute(Request, Token);

    [Fact]
    public async Task Response_Should_NotBeNull() {
        DefaultDependencies();
        var unit = CreateTheUnit();
        ValidArguments();
        var response = await UseTheUnit(unit);
        response.Should().NotBeNull();
    }

    [Fact]
    public async Task Response_Request_Should_NotBeNull() {
        DefaultDependencies();
        var unit = CreateTheUnit();
        ValidArguments();
        var response = await UseTheUnit(unit);
        response.Request.Should().NotBeNull();
    }

    [Fact]
    public async Task Response_Errors_Should_Reflect_Validation_Issues() {
        DefaultDependencies();
        var unit = CreateTheUnit();
        InValidArguments();
        var response = await UseTheUnit(unit);
        response.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Response_Transactions_Should_BeEmpty_If_There_Is_Validation_Issues() {
        DefaultDependencies();
        var unit = CreateTheUnit();
        InValidArguments();
        var response = await UseTheUnit(unit);
        response.Transactions.Should().BeEmpty();
    }

    [Fact]
    public void AddRead_ShouldRegisterDependencies() {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationManager();
        //configuration.AddInMemoryCollection(new Dictionary<string, string?> {
        //    { "ConnectionStrings:App", "Data Source=.\\SQLEXPRESS;Initial Catalog=App;User ID=sa;Password=sa!Password;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False" }
        //});
        // Act
        services.AddFindTransactions(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var feature = serviceProvider.GetService<Service>();

        feature.Should().NotBeNull();
    }
}

