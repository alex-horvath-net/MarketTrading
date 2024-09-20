using Experts.Trader.FindTransactions;
using Clock = Experts.Trader.FindTransactions.Clock.Microsoft;
using EntityFramework = Experts.Trader.FindTransactions.Repository.EntityFramework;
using Flag = Experts.Trader.FindTransactions.Flag.Microsoft;
using FluentValidator = Experts.Trader.FindTransactions.Validator.FluentValidator;

namespace Tests.FindTransactions;

public class Driver {
    public Service.IValidator Validator;
    public Service.IFlag Flag;
    public Service.IRepository Repository;
    public Service.IClock Clock;

    public Service.Request Request;
    public CancellationToken Token;

    public void DefaultDependencies() {
        validatorDriver.SetDefaultDependencies();
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
        validatorDriver.SetValidArguments();
        Request = validatorDriver.Request;
        Token = validatorDriver.Token;
    }

    public void InValidArguments() {
        validatorDriver.SetInValidArguments();
        Request = validatorDriver.Request;
        Token = validatorDriver.Token;
    }

    private readonly EditTransaction.Validator.FluentValidator.Driver validatorDriver = new();
    private readonly Repository.EntityFramework.Driver repositoryDriver = new();
}

