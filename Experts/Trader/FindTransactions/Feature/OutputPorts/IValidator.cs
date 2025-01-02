using Common.Validation.Business.Model;
using Experts.Trader.FindTransactions.Feature;

namespace Experts.Trader.FindTransactions.Feature.OutputPorts;

public interface IValidator { Task<List<Error>> Validate(Request request, CancellationToken token); }

