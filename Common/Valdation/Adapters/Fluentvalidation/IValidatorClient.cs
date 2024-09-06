using Common.Valdation.Adapters.Fluentvalidation.Model;

namespace Common.Valdation.Adapters.Fluentvalidation;

public interface IValidatorClient<TRequest>
{
    Task<List<ErrorModel>> Validate(TRequest request, CancellationToken token);
}
