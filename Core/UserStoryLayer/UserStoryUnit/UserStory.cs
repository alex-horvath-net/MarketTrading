namespace Core.UserStoryLayer.UserStoryUnit;

public class UserStory<TRequest, TResponse>(IEnumerable<ITask<TResponse>> workSteps)                  : IUserStory<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response<Request>, new()
{
    public async Task<TResponse> Run(TRequest request, CancellationToken cancellation)
    {
        var response = new TResponse() with { Request = request };
        foreach (var workStep in workSteps)
        {
            await workStep.Run(response, cancellation);
            cancellation.ThrowIfCancellationRequested();
            if (response.Stopped) return response;
        }
        return response;
    }
}
