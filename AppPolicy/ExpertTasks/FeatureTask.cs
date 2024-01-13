using Core.ExpertStory;
using Core.ExpertStory.DomainModel;

namespace Core.ExpertTasks;

public class FeatureTask<TRequest, TResponse> : IExpertTask<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response<TRequest>, new() {
    public Task Run(TResponse response, CancellationToken token) {
        response.FeatureEnabled = false;
        response.Terminated = !response.FeatureEnabled;
        return Task.CompletedTask;
    }
}
