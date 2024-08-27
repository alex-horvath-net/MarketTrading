using Experts.Trader.ReadTransactions.Business.AdapterPorts;

namespace Experts.Trader.ReadTransactions.Business;
public class Feature(IRepositoryAdapterPort Repository) {
    public async Task<Response> Execute(Request request, CancellationToken token) {
        var response = new Response();
        response.Request = request;
        response.Transactions = await Repository.ReadTransaction(request, token);
        return response;
    }
}
