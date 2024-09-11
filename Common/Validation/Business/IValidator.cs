using Common.Validation.Business.Model;

namespace Common.Validation.Business;

public interface IValidator<TRequest> {
    Task<List<Error>> Validate(TRequest request, CancellationToken token);
}
