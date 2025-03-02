using BusinesActors.Trader.FindTransactions.Feature;

namespace BusinesActors.Trader.FindTransactions.Feature.OutputPorts;

public interface IFlag { bool IsPublic(Request request, CancellationToken token); }

