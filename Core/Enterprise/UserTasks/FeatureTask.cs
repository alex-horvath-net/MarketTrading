using Core.Enterprise.UserStory;

namespace Core.Enterprise.UserTasks;

public class FeatureTask<TRequest, TResponse> : IUserTask<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response<TRequest>, new()
{
    public Task Run(TResponse response, CancellationToken token)
    {
        response.FeatureEnabled = false;
        response.Terminated = !response.FeatureEnabled;
        return Task.CompletedTask;
    }
}
