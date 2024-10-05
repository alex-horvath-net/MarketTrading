using Common.Validation.Business.Model;
using Experts.Trader.FindTransactions.UserStory.InputPort;

namespace Experts.Trader.FindTransactions.UserStory.OutputPort;

public interface IValidator { Task<List<Error>> Validate(Request request, CancellationToken token); }
