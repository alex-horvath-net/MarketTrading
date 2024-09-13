using Common.Adapters.App.Data.Model;
using Experts.Trader.FindTransactions;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Clock = Experts.Trader.FindTransactions.Clock.Microsoft;
using EntityFramework = Experts.Trader.FindTransactions.Repository.EntityFramework;
using Flag = Experts.Trader.FindTransactions.Flag.Microsoft;
using FluentValidator = Experts.Trader.FindTransactions.Validator.FluentValidator;

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

public class Driver {
    public Service.IValidator Validator;
    public Service.IFlag Flag;
    public Service.IRepository Repository;
    public Service.IClock Clock;

    public Service.Request Request;
    public CancellationToken Token;

    public void DefaultDependencies() {
        validatorDriver.DefaultDependencies();
        var validatorClient = validatorDriver.Client;
        Validator = new FluentValidator.Adapter(validatorClient);

        var flagClient = new Flag.Client();
        Flag = new Flag.Adapter(flagClient);

        repositoryDriver.LightDependencies();
        var repositoryClient = repositoryDriver.Client; // CreateFakeRepositoryClient();
        Repository = new EntityFramework.Adapter(repositoryClient);

        var clockClient = new Clock.Client();
        Clock = new Clock.Adapter(clockClient);
    }

    public void ValidArguments() {
        validatorDriver.ValidArguments();
        Request = validatorDriver.Request;
        Token = validatorDriver.Token;
    }

    public void InValidArguments() {
        validatorDriver.InValidArguments();
        Request = validatorDriver.Request;
        Token = validatorDriver.Token;
    }

    private readonly Validator.FluentValidator.Driver validatorDriver = new();
    private readonly Repository.EntityFramework.Driver repositoryDriver = new();

}

