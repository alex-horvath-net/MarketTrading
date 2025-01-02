using Infrastructure.Validation.Business.Model;

namespace Infrastructure.Validation.Business;

public interface IValidator<TRequest> {
    Task<List<Error>> Validate(TRequest request, CancellationToken token);
}
