using Common.Valdation.Business;
using Experts.Trader.FindTransactions.Read.Business;

namespace Experts.Trader.FindTransactions;

public class Service(IValidatorAdapter<Request> validator, IRepositoryAdapter repository) {
    public async Task<Response> Execute(Request request, CancellationToken token) {
        var response = new Response();

        response.Request = request;

        response.Errors = await validator.Validate(request, token);
        if (response.Errors.Count > 0)
            return response;

        response.Transactions = await repository.ReadTransaction(request, token);

        return response; 
    }
}
