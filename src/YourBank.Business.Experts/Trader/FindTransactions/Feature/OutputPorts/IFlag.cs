using YourBank.Business.Experts.Trader.FindTransactions.Feature;

namespace YourBank.Business.Experts.Trader.FindTransactions.Feature.OutputPorts;

public interface IFlag { bool IsPublic(Request request, CancellationToken token); }

