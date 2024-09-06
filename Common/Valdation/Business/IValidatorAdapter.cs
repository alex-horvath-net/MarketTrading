using Common.Valdation.Business.Model;

namespace Common.Valdation.Business;

public interface IValidatorAdapter<TRequest> {
    Task<List<Error>> Validate(TRequest request, CancellationToken token);
}
