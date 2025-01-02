using Infrastructure.Validation.FluentValidator.Adapters.Model;

namespace Infrastructure.Validation.FluentValidator.Adapters;

public interface ICommonClient<TRequest> {
    Task<List<Model>> Validate(TRequest request, CancellationToken token);
}

