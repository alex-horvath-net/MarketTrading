using Experts.Trader.EditTransaction;
using EntityFramework = Experts.Trader.EditTransaction.Repository.EntityFramework;
using FluentValidator = Experts.Trader.EditTransaction.Validator.FluentValidator;

namespace Tests.EditTransaction;

public class Driver {
    public Service.IValidator Validator;
    public Service.IRepository Repository;

    public Service.Request Request;
    public CancellationToken Token;

    public void DefaultDependencies() {
        validatorDriver.DefaultDependencies();
        var validatorClient = validatorDriver.Client;
        Validator = new FluentValidator.Adapter(validatorClient);

        repositoryDriver.LightDependencies();
        var repositoryClient = repositoryDriver.Client; // CreateFakeRepositoryClient();
        Repository = new EntityFramework.Adapter(repositoryClient);

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

    private readonly EditTransaction.Validator.FluentValidator.Driver validatorDriver = new();
    private readonly Repository.EntityFramework.Driver repositoryDriver = new();
}

