using Common.Business;

namespace Experts.Trader.ReadTransactions;

public class Feature(
    Feature.IValidatorAdapterPort Validator,
    Feature.IRepositoryAdapterPort Repository) {
    public async Task<Response> Execute(Request request, CancellationToken token) {
        var response = new Response();

        response.Request = request;

        response.Errors = await Validator.Validate(request, token);
        if (response.Errors.Count > 0)
            return response;

        response.Transactions = await Repository.ReadTransaction(request, token);

        return response;
    }

    public class Request {
        public string Name { get; set; }
    }
   
    public class Response  {
        public Request Request { get; set; }
        public List<string> Errors { get; set; } = [];
        public List<Transaction> Transactions { get; set; } = [];
    }

    public interface IValidatorAdapterPort {
        public Task<List<string>> Validate(Request request, CancellationToken token);
    }

    public interface IRepositoryAdapterPort {
        public Task<List<Common.Business.Transaction>> ReadTransaction(Request request, CancellationToken token);
    }
}
