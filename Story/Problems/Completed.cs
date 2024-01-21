using Common.Model;

namespace Common.Problems;

public class Completed<TRequest, TResponse> //: IProblem<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response<TRequest>, new() {

    public Task Run(TResponse response, CancellationToken token) {
        response.EndAt = DateTime.UtcNow;
        return Task.CompletedTask;
    }
}
