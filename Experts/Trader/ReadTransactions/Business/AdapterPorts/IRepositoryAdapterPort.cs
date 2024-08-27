using Experts.Trader.ReadTransactions.Business;
using BusinessModel = Common.Business;

namespace Experts.Trader.ReadTransactions.Business.AdapterPorts;

public interface IRepositoryAdapterPort {
    public Task<List<BusinessModel.Transaction>> ReadTransaction(Request request, CancellationToken token);
}