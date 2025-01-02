using Experts.Trader.FindTransactions.Feature;

namespace Experts.Trader.FindTransactions.Feature.OutputPorts;

public interface IFlag { bool IsPublic(Request request, CancellationToken token); }

