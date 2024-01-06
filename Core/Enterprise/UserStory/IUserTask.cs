namespace Core.Enterprise.UserStory;

public interface IUserTask<TRequest, TResponse>
    where TRequest : RequestCore
    where TResponse : Response<TRequest>, new()
{
    Task<bool> Run(TResponse response, CancellationToken token);
}

