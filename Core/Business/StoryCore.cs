namespace Core.Business;

public interface IStory<TRequest, TResponse>
    where TRequest : RequestCore
    where TResponse : ResponseCore<TRequest>, new() {
    Task<TResponse> Run(TRequest request, CancellationToken token);
}

public class StoryCore<TRequest, TResponse>(IValidation<TRequest> validator) : IStory<TRequest, TResponse>
    where TRequest : RequestCore
    where TResponse : ResponseCore<TRequest>, new() {
    public async Task<TResponse> Run(TRequest request, CancellationToken token) {
        var response = new TResponse();

        response.StartedAt = DateTime.UtcNow;

        response.Request = request;

        response.FeatureEnabled = true;
        if (!response.FeatureEnabled)
            return response;

        response.ValidationResults = await validator.Validate(response.Request, token);
        if (response.ValidationResults.HasIssue())
            return response;
        
        await RunCore(response, token);

        response.CompletedAt = DateTime.UtcNow;

        return response;
    }

    public async Task BusinessTask(string name, TResponse response, Func<TResponse, Task> task) {

        await task(response);
    }

    public virtual Task RunCore(TResponse response, CancellationToken token) => Task.CompletedTask;
}


public interface IValidation<TRequest> where TRequest : RequestCore {
    Task<IEnumerable<ValidationResult>> Validate(TRequest request, CancellationToken token);
}

