using Core.Enterprise.UserStory;

namespace Core.Enterprise.UserTasks;

public class FeatureTask<TRequest, TResponse> : IUserTask<TRequest, TResponse>
    where TRequest : RequestCore
    where TResponse : Response<TRequest>, new()
{
    public Task<bool> Run(TResponse response, CancellationToken token)
    {
        response.FeatureEnabled = false;
        return (!response.FeatureEnabled).ToTask();
    }
}
