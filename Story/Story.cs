namespace Common;

public class Story<TRequest, TResponse>() : IStory<TRequest, TResponse>
    where TRequest : Model.Request
    where TResponse : Model.Response<TRequest>, new() {
    public async Task<TResponse> Run(TRequest request, CancellationToken token) {
        var response = new TResponse();

        response.Request = request;
        response.StartedAt = DateTime.UtcNow;
        response.FeatureEnabled = false;
        response.Terminated = !response.FeatureEnabled;

        await RunCore(response, token);

        response.EndAt = DateTime.UtcNow;

        return response;
    }

    public async Task BusinessTask(string name, TResponse response, Func<TResponse, Task> task) {

        await task(response);
    }

    public virtual Task RunCore(TResponse response, CancellationToken token) => Task.CompletedTask;
}
