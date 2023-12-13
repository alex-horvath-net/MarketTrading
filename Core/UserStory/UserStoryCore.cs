namespace Core.UserStory;

public class UserStoryCore<TRequest, TResponse>(IEnumerable<ITask<TRequest, TResponse>> tasks) : IUserStory<TRequest, TResponse>
    where TRequest : RequestCore
    where TResponse : ResponseCore<TRequest>, new()
{
    public async Task<TResponse> Run(TRequest request, CancellationToken token)
    {
        var response = new TResponse() with { Request = request };
        foreach (var task in tasks)
        {
            if (response.Stopped) return response;
            await task.Run(response, token);
            token.ThrowIfCancellationRequested();
        }
        return response;
    }
}
