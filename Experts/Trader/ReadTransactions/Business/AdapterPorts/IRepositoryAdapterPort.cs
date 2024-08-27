namespace Experts.Trader.ReadTransactions.Business.AdapterPorts;

public interface IRepositoryAdapterPort {
    public Task<List<Common.Business.Transaction>> ReadTransaction(Request request, CancellationToken token);
}