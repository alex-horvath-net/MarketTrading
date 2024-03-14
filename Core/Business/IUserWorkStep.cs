using Core.Business.Model;

namespace Core.Business;

public interface IUserWorkStep<TRequest, TResponse>
    where TRequest : RequestCore
    where TResponse : ResponseCore<TRequest>, new() {
    Task<bool> Run(TResponse response, CancellationToken token);
    string Name { get; }
}


public abstract class UserWorkStep<TRequest, TResponse> : IUserWorkStep<TRequest, TResponse>
    where TRequest : RequestCore where TResponse : ResponseCore<TRequest>, new() {

    public virtual Task<bool> Run(TResponse response, CancellationToken token) => true.ToTask();


    public static UserWorkStep<TRequest, TResponse> operator +(
        UserWorkStep<TRequest, TResponse> left,
        UserWorkStep<TRequest, TResponse> right) {
        return left;
    }

    public string Name => name ??= GetType().Name;
    private string? name;
}

