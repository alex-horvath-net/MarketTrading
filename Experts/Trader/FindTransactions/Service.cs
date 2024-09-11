using Experts.Trader.FindTransactions.Clock.Business;
using Experts.Trader.FindTransactions.Flag.Busines;
using Experts.Trader.FindTransactions.Validator.Business;

namespace Experts.Trader.FindTransactions;

public class Service(IValidator validator, IFlag flag, Repository.IRepository repository, IClock clock)
{
    public async Task<Response> Execute(Request request, CancellationToken token)
    {
        var response = new Response();
        try
        {
            response.Errors = await validator.Validate(request, token);
            if (response.Errors.Count > 0)
                return response;

            response.IsPublic = flag.IsPublic(request, token);

            response.Request = request;

            response.Transactions = await repository.FindTransactions(request, token);

        }
        catch (Exception ex)
        {
            response.Exception = ex;
        }
        finally
        {
            response.StopedAt = clock.GetTime();
        }
        return response;
    }
}
