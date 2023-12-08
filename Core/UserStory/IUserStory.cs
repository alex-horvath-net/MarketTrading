namespace Sys.UserStory;

public interface IUserStory<TRequest, TResponse>
{
    Task<TResponse> Run(TRequest request, CancellationToken cancellation);
}