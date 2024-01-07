namespace Core.Sys.UserStory;

public interface IUserTask<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response<TRequest>, new()
{
    Task Run(TResponse response, CancellationToken token);
}

