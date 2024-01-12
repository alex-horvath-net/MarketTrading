using Core.ExpertStory;
using Core.ExpertStory.DomainModel;

namespace Core.UserTasks;

public class FeatureTask<TRequest, TResponse> : IScope<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response<TRequest>, new() {
    public Task Run(TResponse response, CancellationToken token) {
        response.FeatureEnabled = false;
        response.Terminated = !response.FeatureEnabled;
        return Task.CompletedTask;
    }
}
