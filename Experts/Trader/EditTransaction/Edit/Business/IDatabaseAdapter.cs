using Common.Business.Model;

namespace Experts.Trader.EditTransaction.Edit.Business;

public interface IDatabaseAdapter {
    Task<Transaction?> EditTransaction(Request request, CancellationToken token);
}