using Common.Valdation.Business;
using Experts.Trader.EditTransaction.Edit.Business;

namespace Experts.Trader.EditTransaction;

public class Service(IValidatorAdapter<Request> validator, IRepositoryAdapter repository) {
    public async Task<Response> Execute(Request request, CancellationToken token) {
        var response = new Response();

        response.Request = request;

        response.Errors = await validator.Validate(request, token);
        if (response.Errors.Count > 0)
            return response;

        response.Transaction = await repository.EditTransaction(request, token);

        return response;
    }
}
