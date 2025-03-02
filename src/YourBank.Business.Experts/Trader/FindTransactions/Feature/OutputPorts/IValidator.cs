using YourBank.Business.Experts.Trader.FindTransactions.Feature;
using YourBank.Infrastructure.Validation.Business.Model;

namespace YourBank.Business.Experts.Trader.FindTransactions.Feature.OutputPorts;

public interface IValidator { Task<List<Error>> Validate(Request request, CancellationToken token); }

