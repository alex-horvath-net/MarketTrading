using Infrastructure.Validation.Business.Model;

namespace DomainExperts.Trader.FindTransactions.Feature.OutputPorts;

public interface IValidator { Task<List<Error>> Validate(Request request, CancellationToken token); }

