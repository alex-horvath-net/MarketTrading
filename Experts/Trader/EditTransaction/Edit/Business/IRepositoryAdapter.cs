using Common.Data.Business.Model;

namespace Experts.Trader.EditTransaction.Edit.Business;

public interface IRepositoryAdapter {
    Task<Transaction?> EditTransaction(Request request, CancellationToken token);
}