using Core.Plugins.FP;
using Core.UserStory;

namespace Core.Tasks;

public class FeatureTask<TRequest, TResponse> : ITask<TRequest, TResponse>
    where TRequest : RequestCore
    where TResponse : ResponseCore<TRequest>, new()
{
    public async Task Run(TResponse response, CancellationToken token)
    {
        response.FeatureFlag = await false.ToTask();
        response.CanRun = !response.FeatureFlag;
    }
}
