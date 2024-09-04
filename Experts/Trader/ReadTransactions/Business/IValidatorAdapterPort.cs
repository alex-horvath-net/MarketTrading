namespace Experts.Trader.ReadTransactions.Business;

public interface IValidatorAdapterPort {
    public Task<List<string>> Validate(Request request, CancellationToken token);
}
