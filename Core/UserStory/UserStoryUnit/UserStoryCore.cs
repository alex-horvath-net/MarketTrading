namespace Core.UserStory.UserStoryUnit;

public abstract class UserStoryCore<TRequest, TResponse>(IEnumerable<ITask<TRequest, TResponse>> workSteps) : IUserStory<TRequest, TResponse>
    where TRequest : RequestCore
    where TResponse : ResponseCore<TRequest>, new()
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
