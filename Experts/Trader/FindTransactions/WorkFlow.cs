using Common.Data.Business.Model;
using Common.Valdation.Business;
using Common.Valdation.Business.Model;
using Experts.Trader.FindTransactions.Read.Business;

namespace Experts.Trader.FindTransactions;

public class WorkFlow(IValidatorAdapter<Request> validator, IRepositoryAdapter repository) {
    public async Task<Response> Execute(Request request, CancellationToken token) {
        var response = new Response();

        response.Request = request;
         
        response.Errors = await validator.Validate(request, token);
        if (response.Errors.Count > 0)
            return response;

        response.Transactions = await repository.FindTransactions(request, token);

        return response; 
    }
}

public class Request {
    public string? Name { get; set; }
}


public class Response {
    public Request? Request { get; set; }
    public List<Error> Errors { get; set; } = [];
    public List<Transaction> Transactions { get; set; } = [];
}
