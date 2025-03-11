namespace Business.Experts.Trader.FindTransactions.Feature;

public interface IFeature {
    Task<Response> Execute(Request request, CancellationToken token);
}

