using YourBank.Business.Domain;
using YourBank.Business.Experts.Trader.FindTransactions.Feature;

namespace YourBank.Business.Experts.Trader.FindTransactions.Feature.OutputPorts;

public interface IRepository { Task<List<Trade>> FindTransactions(Request request, CancellationToken token); }

