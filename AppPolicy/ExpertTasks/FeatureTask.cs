using Core.Story;
using Core.Story.StoryModel;

namespace Core.ExpertTasks;

public class FeatureTask<TRequest, TResponse> : IProblem<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response<TRequest>, new() {
    public Task Run(TResponse response, CancellationToken token) {
        response.FeatureEnabled = false;
        response.Terminated = !response.FeatureEnabled;
        return Task.CompletedTask;
    }
}
