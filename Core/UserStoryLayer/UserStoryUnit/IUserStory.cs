namespace Principals.UserStoryLayer.UserStoryUnit;

public interface IUserStory<TRequest, TResponse>
{
    Task<TResponse> Run(TRequest request, CancellationToken cancellation);
}
