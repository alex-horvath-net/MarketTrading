namespace Core.Business;

public interface IValidator<TRequest> where TRequest : RequestCore {
    Task<IEnumerable<ValidationResult>> Validate(TRequest request, CancellationToken token);
}

