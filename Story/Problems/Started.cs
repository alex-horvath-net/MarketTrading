using Common.Model;

namespace Common.Problems;

public class Started<TRequest, TResponse> //: IProblem<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response<TRequest>, new() {
    public Task Run(TResponse response, CancellationToken token) {
        response.StartedAt = DateTime.UtcNow;
        return Task.CompletedTask;
    }
}
