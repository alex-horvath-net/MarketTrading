
using Domain;

namespace Trader.Transactions.ReadTransactions;
public class Feature(Feature.IRepository Repository) {
    public async Task<Response> Execute(Request request, CancellationToken token) {
        var response = new Response();
        response.Request = request;
        response.Transactions = Repository.Read(token);
        return await Task.FromResult(response);
    }

    public class Request {
    }

    public class Response {
        public Request Request { get; set; }
        public List<Transaction> Transactions { get; set; }
    }

    public interface IRepository {
        public List<Transaction> Read(CancellationToken token);
    }
}