namespace Common.Validation.FluentValidator.Adapters;

public interface ICommonClient<TRequest>
{
    Task<List<Model.Model>> Validate(TRequest request, CancellationToken token);
}

