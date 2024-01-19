using Core.Story;
using Core.Story.Model;

namespace Core.Problems;

public class FeatureEnabled<TRequest, TResponse> : IProblem<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response<TRequest>, new() {
    public Task Run(TResponse response, CancellationToken token) {
        response.FeatureEnabled = false;
        response.Terminated = !response.FeatureEnabled;
        return Task.CompletedTask;
    }
}
