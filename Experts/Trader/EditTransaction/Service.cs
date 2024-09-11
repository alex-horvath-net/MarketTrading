using Common.Validation.Business;
using Experts.Trader.EditTransaction.Edit.Business;

namespace Experts.Trader.EditTransaction;

public class Service(IValidator<Request> validator, IDatabaseAdapter repository) {
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
