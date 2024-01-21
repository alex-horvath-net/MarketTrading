using Story.Model;

namespace Story.Problems;

public class FeatureEnabled<TRequest, TResponse> //: IProblem<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response<TRequest>, new() {
    public int Order => 0;
    public Task Run(TResponse response, CancellationToken token) {
        response.FeatureEnabled = false;
        response.Terminated = !response.FeatureEnabled;
        return Task.CompletedTask;
    }
}
