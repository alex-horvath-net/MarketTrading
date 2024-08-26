
using Domain;

namespace Trader.Transactions.ReadTransactions;
public class Feature(Feature.IRepository Repository) {
    public async Task<Response> Execute(Request request, CancellationToken token) {
        var response = new Response();
        response.Request = request;
        response.Transactions = await Repository.Read(request, token);
        return response;
    }

    public class Request {
    }

    public class Response {
        public Request Request { get; set; }
        public List<Transaction> Transactions { get; set; }
    }

    public interface IRepository {
        public Task<List<Transaction>> Read(Request request, CancellationToken token);
    }
}