using Business.Domain;
using Business.Experts.Trader.FindTransactions.Feature;

namespace Business.Experts.Trader.FindTransactions.Feature.OutputPorts;

public interface IRepository { Task<List<Trade>> FindTransactions(Request request, CancellationToken token); }

