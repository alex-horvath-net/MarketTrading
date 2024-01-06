namespace Core.Enterprise.UserStory;

public interface IUserTask<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response<TRequest>, new()
{
    Task Run(TResponse response, CancellationToken token);
}

