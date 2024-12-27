using Common.Validation.Business.Model;
using DomainExperts.Trader.FindTransactions.UserStory.InputPort;

namespace DomainExperts.Trader.FindTransactions.UserStory.OutputPort;

public interface IValidator { Task<List<Error>> Validate(Request request, CancellationToken token); }
