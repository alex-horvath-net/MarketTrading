using Business.Experts.Trader.FindTransactions.Feature;
using Infrastructure.Validation.Business.Model;

namespace Business.Experts.Trader.FindTransactions.Feature.OutputPorts;

public interface IValidator { Task<List<Error>> Validate(Request request, CancellationToken token); }

