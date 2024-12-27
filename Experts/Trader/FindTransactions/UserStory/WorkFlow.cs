using DomainExperts.Trader.FindTransactions.UserStory.InputPort;
using DomainExperts.Trader.FindTransactions.UserStory.OutputPort;

namespace DomainExperts.Trader.FindTransactions.UserStory;

public class WorkFlow(IValidator validator, IFlag flag, IRepository repository, IClock clock) : IUserStory {

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
