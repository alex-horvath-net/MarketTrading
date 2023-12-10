namespace Core.UserStoryLayer.UserStoryUnit;

public interface IUserStory<TRequest, TResponse> 
    where TRequest : RequestCore
    where TResponse : ResponseCore<TRequest>, new()
{
    Task<TResponse> Run(TRequest request, CancellationToken cancellation);
}
