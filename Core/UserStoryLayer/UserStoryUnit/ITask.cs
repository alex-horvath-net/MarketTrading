namespace Core.UserStoryLayer.UserStoryUnit;

public interface ITask<TResponse> where TResponse : Response<Request>
{
    Task Run(TResponse response, CancellationToken cancellation);
}
