using Common.Adapters.App.Data.Model;
using Experts.Trader.FindTransactions;
using Validator = Experts.Trader.FindTransactions.Validator;
using Repository = Experts.Trader.FindTransactions.Repository;
using Clock = Experts.Trader.FindTransactions.Clock;
using Flag = Experts.Trader.FindTransactions.Flag;
using FluentValidation;




namespace Tests;
public class Helper
{

    public IValidator<Service.Request> ValidatorTechnology;
    Func<IValidator<Service.Request>> validatorTechnology;

    public Validator.FluentValidator.Adapter.IClient ValidatorClient;
    Func<Validator.FluentValidator.Adapter.IClient> validatorClient;

    public Service.IValidator ValidatorAdapter;
    Func<Service.IValidator> validatorAdapter;

    public Flag.Microsoft.Adapter.IClient FlagClient;
    Func<Flag.Microsoft.Adapter.IClient> flagClient;

    public Service.IFlag FlagAdapter;
    Func<Service.IFlag> flagAdapter;

    public Repository.EntityFramework.Adapter.IClient RepositoryClient;
    Func<Repository.EntityFramework.Adapter.IClient> repositoryClient;

    public Service.IRepository RepositoryAdapter;
    Func<Service.IRepository> repositoryAdapter;

    public Clock.Microsoft.Adapter.IClient ClockClient;
    Func<Clock.Microsoft.Adapter.IClient> clockClient;

    public Service.IClock ClockAdapter;
    Func<Service.IClock> clockAdapter;


    public Helper Default()
    {
        validatorTechnology = () => new Validator.FluentValidator.Technology();
        validatorClient = () => new Validator.FluentValidator.Client(ValidatorTechnology);
        validatorAdapter = () => new Validator.FluentValidator.Adapter(ValidatorClient);

        flagClient = () => new Flag.Microsoft.Client();
        flagAdapter = () => new Flag.Microsoft.Adapter(FlagClient);

        repositoryClient = () => new Repository.EntityFramework.Client(DatabaseFactory.Default());
        repositoryAdapter = () => new Repository.EntityFramework.Adapter(RepositoryClient);

        clockClient = () => new Clock.Microsoft.Client();
        clockAdapter = () => new Clock.Microsoft.Adapter(ClockClient);

        return this;
    }


    public Helper UseFakeRepositoryClient()
    {
        var repositoryTechnology = new List<TransactionDM>() {
                new() { Id = 1, Name = "USD" }
            };
        repositoryClient = () => new FakeRepositoryClient(repositoryTechnology);
        return this;
    }

    public Helper BuildServiceDependency()
    {
        ValidatorTechnology = validatorTechnology();
        ValidatorClient = validatorClient();
        ValidatorAdapter = validatorAdapter();

        FlagClient = flagClient();
        FlagAdapter = flagAdapter();

        RepositoryClient = repositoryClient();
        RepositoryAdapter = repositoryAdapter();

        ClockClient = clockClient();
        ClockAdapter = clockAdapter();

        return this;
    }
}

public class FakeRepositoryClient(List<TransactionDM> fakeDB) : Repository.EntityFramework.Adapter.IClient
{
    public Task<List<TransactionDM>> Find(string? name, CancellationToken token) => Task.FromResult(
        fakeDB.Where(x => x.Name == name).ToList());
}

public record Arguments(Service.Request Request, CancellationToken Token)
{
    public static Arguments Valid() => new(
        new() { UserId = "alad", Name = "USD" },
        CancellationToken.None);

    public static Arguments InValid() => new(
      new() { Name = "US" },
      CancellationToken.None);
}

