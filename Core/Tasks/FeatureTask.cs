namespace Core.Tasks;

public class FeatureTask<TRequest, TResponse> : Core.UserStory.ITask<TRequest, TResponse>
    where TRequest : Core.UserStory.RequestCore
    where TResponse : Core.UserStory.ResponseCore<TRequest>, new()
{
    public async Task Run(TResponse response, CancellationToken token)
    {
        response.FeatureFlag = await false.ToTask();
        response.CanRun = !response.FeatureFlag;
    }
}
