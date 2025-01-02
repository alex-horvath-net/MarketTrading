using DomainExperts.Trader.FindTransactions.Feature;

namespace DomainExperts.Trader.FindTransactions.Feature.OutputPorts;

public interface IFlag { bool IsPublic(Request request, CancellationToken token); }

