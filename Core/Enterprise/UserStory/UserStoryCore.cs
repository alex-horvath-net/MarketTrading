namespace Core.Enterprise.UserStory;

public class UserStoryCore<TRequest, TResponse>(IEnumerable<IUserTask<TRequest, TResponse>> userTasks) : IUserStory<TRequest, TResponse>
    where TRequest : RequestCore
    where TResponse : ResponseCore<TRequest>, new()
{
    public async Task<TResponse> Run(TRequest request, CancellationToken token)
    {
        var response = new TResponse() { Request = request };
        foreach (var userTask in userTasks)
        {
            var terminated = await userTask.Run(response, token);
            if (terminated)
                break;
        }
        return response;
    }
}
