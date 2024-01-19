using Core.Story;
using Core.Story.Model;

namespace Core.Problems;

public class Started<TRequest, TResponse> : IProblem<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response<TRequest>, new() {
    public Task Run(TResponse response, CancellationToken token) {
        response.StartedAt = DateTime.UtcNow;
        return Task.CompletedTask;
    }
}
