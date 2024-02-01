namespace Core.Business;

public interface IValidator<TRequest> where TRequest : RequestCore {
    Task<IEnumerable<Result>> Validate(TRequest request, CancellationToken token);
}

