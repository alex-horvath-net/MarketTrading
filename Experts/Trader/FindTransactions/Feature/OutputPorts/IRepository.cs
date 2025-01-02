using Experts.Trader.FindTransactions.Feature;
using Infrastructure.Business.Model;

namespace Experts.Trader.FindTransactions.Feature.OutputPorts;

public interface IRepository { Task<List<Trade>> FindTransactions(Request request, CancellationToken token); }

