using YourBank.Infrastructure.Validation.FluentValidator.Adapters.Model;

namespace YourBank.Infrastructure.Validation.FluentValidator.Adapters;

public interface ICommonClient<TRequest> {
    Task<List<Model.Model>> Validate(TRequest request, CancellationToken token);
}

