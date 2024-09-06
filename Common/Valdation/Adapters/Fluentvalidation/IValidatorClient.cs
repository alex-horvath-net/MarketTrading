namespace Common.Valdation.Adapters.Fluentvalidation;

public interface IValidatorClient<TRequest>
{
    Task<List<FluentvalidationErrorModel>> Validate(TRequest request, CancellationToken token);
}
