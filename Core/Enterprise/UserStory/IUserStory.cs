namespace Core.Enterprise.UserStory;

public interface IUserStory<TRequest, TResponse>
    where TRequest : RequestCore
    where TResponse : Response<TRequest>, new()
{
    Task<TResponse> Run(TRequest request, CancellationToken token);
}
