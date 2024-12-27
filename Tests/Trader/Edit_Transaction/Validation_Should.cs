using Common.Technology;
using Common.Validation.Business.Model;
using DomainExperts.Trader.EditTransaction;
using DomainExperts.Trader.EditTransaction.WorkSteps;

namespace Tests.Trader.Edit_Transaction;

public class Validation_Should {
    public Valiadator.BusinessAdapter.ITechnologyAdapter TechnologyAdapter;
    public Repository.BusinessAdapter.ITechnologyAdapter RepositoryTechnologyAdapter;
    public BusinessNeed.IValidator Unit;
    public void Crea_The_Unit() => Unit = new Valiadator.BusinessAdapter(TechnologyAdapter);

    public List<Error> Response;
    public BusinessNeed.Request Request;
    public CancellationToken Token;
    public async Task Use_The_Unit() => Response = await Unit.Validate(Request, Token);

    [Xunit.Fact]
    public async Task Provide_No_Errors_For_Valid_Request() {
        Create_Fast_Dependencies();
        Crea_The_Unit();
        Create_Valid_Arguments();
        await Use_The_Unit();
        Response.Should().BeEmpty();
    }

    [Xunit.Fact]
    public async Task Provide_Errors_For_Non_Valid_Request() {
        Create_Fast_Dependencies();
        Crea_The_Unit();
        Create_Non_Valid_Arguments();
        await Use_The_Unit();
        Response.Should().NotBeEmpty();
    }

    [IntegrationFactAttribute]
    public void Use_DI() {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationManager();
        configuration.AddInMemoryCollection(new Dictionary<string, string?> {
            { "ConnectionStrings:App", "" },
            { "ConnectionStrings:Identity", "" }
        });
        // Act
        services.AddCommonTechnology(configuration);
        services.AddRepositoryAdapter();
        services.AddValidatorAdapter();

        // Assert
        var sp = services.BuildServiceProvider();
        sp.GetRequiredService<BusinessNeed.IValidator>().Should().NotBeNull();
        sp.GetRequiredService<Valiadator.BusinessAdapter.ITechnologyAdapter>().Should().NotBeNull();
        sp.GetRequiredService<FluentValidation.IValidator<BusinessNeed.Request>>().Should().NotBeNull();
    }

    public Validation_Should Create_Fast_Dependencies() {

        if (RepositoryTest == null) {
            RepositoryTest = new Repository_Should();
            RepositoryTest.Create_Fast_Dependencies();
        }

        RepositoryTechnologyAdapter = RepositoryTest.TechnologyAdapter;

        var technology = new  Valiadator.Technology(RepositoryTechnologyAdapter);
        TechnologyAdapter = new Valiadator.TechnologyAdapter(technology);
        return this;
    }

    public void Create_Valid_Arguments() {
        Request = new() { UserId = "aladar", TransactionId = 2, Name = "USD_NEW" };
        Token = CancellationToken.None;
    }

    public void Create_Non_Valid_Arguments() {
        Request = new() { UserId = "aladar", TransactionId = -2, Name = "US" };
        Token = CancellationToken.None;
    }

    public Repository_Should RepositoryTest;
}
