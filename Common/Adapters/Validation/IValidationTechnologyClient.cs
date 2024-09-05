using Common.Adapters.Validation.Model;

namespace Common.Adapters.Validation;

public interface IValidationTechnologyClient<TRequest> {
    public Task<List<ErrorTM>> Validate(TRequest request, CancellationToken token);
}



 