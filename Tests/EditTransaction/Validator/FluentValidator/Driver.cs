using Experts.Trader.EditTransaction;
using Experts.Trader.EditTransaction.Validator.FluentValidator;

namespace Tests.EditTransaction.Validator.FluentValidator;

public class Driver {
    public Adapter.IClient Client;

    public Service.Request Request;
    public CancellationToken Token;

    public void DefaultDependencies() {
        var technology = new Technology();
        Client = new Client(technology);
    }

    public void ValidArguments() {
        Request = new() { UserId = "aladar", Name = "USD" };
        Token = CancellationToken.None;
    }

    public void InValidArguments() {
        Request = new() { UserId = "aladar", Name = "US" };
        Token = CancellationToken.None;
    }
}