using Experts.Trader.FindTransactions.Feature.OutputPorts;

namespace Experts.Trader.FindTransactions.Feature;

public class Featrure(IValidator validator, IFlag flag, IRepository repository, IClockAdapter clock) : IFeature {

    public async Task<Response> Execute(Request request, CancellationToken token) {
        var response = new Response();
        try {
            response.IsUnderConstruction = flag.IsPublic(request, token);
            response.Request = request;
            response.Errors = await validator.Validate(request, token);
            if (response.Errors.Count > 0)
                return response;


            response.Transactions = await repository.FindTransactions(request, token);

            token.ThrowIfCancellationRequested();
        } catch (Exception ex) {
            response.Exception = ex;
        } finally {
            response.StopedAt = clock.GetTime();
        }
        return response;
    }
}

