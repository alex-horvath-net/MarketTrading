using YourBank.Infrastructure.Validation.Business.Model;

namespace YourBank.Infrastructure.Validation.Business;

public interface IValidator<TRequest> {
    Task<List<Error>> Validate(TRequest request, CancellationToken token);
}
