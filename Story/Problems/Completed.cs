namespace Story.Problems;

public class Completed<TRequest, TResponse> //: IProblem<TRequest, TResponse>
    where TRequest : Model.Request
    where TResponse : Model.Response<TRequest>, new() {

    public Task Run(TResponse response, CancellationToken token) {
        response.EndAt = DateTime.UtcNow;
        return Task.CompletedTask;
    }
}
