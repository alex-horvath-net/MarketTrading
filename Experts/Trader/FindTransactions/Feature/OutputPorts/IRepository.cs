using Common.Business.Model;
using Experts.Trader.FindTransactions.Feature;

namespace Experts.Trader.FindTransactions.Feature.OutputPorts;

public interface IRepository { Task<List<Transaction>> FindTransactions(Request request, CancellationToken token); }

