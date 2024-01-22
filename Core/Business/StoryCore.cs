namespace Core.Business;

public interface IStory<TRequest, TResponse, TStory>
    where TRequest : RequestCore
    where TResponse : ResponseCore<TRequest>, new()
    where TStory : class {
    Task<TResponse> Run(TRequest request, CancellationToken token);
}

public class StoryCore<TRequest, TResponse, TStory>(
    IValidator<TRequest> validator,
    ILogger<TStory> logger) : IStory<TRequest, TResponse, TStory>
    where TRequest : RequestCore
    where TResponse : ResponseCore<TRequest>, new()
    where TStory : class {

    private string Name = nameof(TStory);

    public async Task<TResponse> Run(TRequest request, CancellationToken token) {
        var response = new TResponse();
        try {
            response.StartedAt = DateTime.UtcNow;
            logger.LogInformation("Event {Event}, Story {Story}, Time {Time}", "Started", Name, response.StartedAt);

            response.Request = request;

            response.FeatureEnabled = true;
            if (!response.FeatureEnabled)
                return response;

            response.ValidationResults = await validator.Validate(response.Request, token);
            if (response.ValidationResults.HasIssue())
                return response;

            await RunCore(response, token);

            response.CompletedAt = DateTime.UtcNow;
            logger.LogInformation("Event {Event}, Story {Story}, Time {Time}", "Completed", Name, response.CompletedAt);
        } catch (Exception ex) {
            logger.LogError(ex, "Event {Event}, Story {Story}, Time {Time}", "Failed", Name, DateTime.UtcNow);
            throw;
        }

        return response;
    }

    public async Task BusinessTask(string name, TResponse response, Func<TResponse, Task> task) {

        await task(response);
    }

    public virtual Task RunCore(TResponse response, CancellationToken token) => Task.CompletedTask;
}


public interface IValidator<TRequest> where TRequest : RequestCore {
    Task<IEnumerable<ValidationResult>> Validate(TRequest request, CancellationToken token);
}

public interface ILogger<T> where T : class {
    void LogInformation(string messageTemplate);
    void LogInformation<P0>(string messageTemplate, P0 p0);
    void LogInformation<P0, P1>(string messageTemplate, P0 p0, P1 p1);
    void LogInformation<P0, P1, P2>(string messageTemplate, P0 p0, P1 p1, P2 p2);

    void LogError(Exception exception, string messageTemplate);
    void LogError<P0>(Exception exception, string messageTemplate, P0 p0);
    void LogError<P0, P1>(Exception exception, string messageTemplate, P0 p0, P1 p1);
    void LogError<P0, P1, P2>(Exception exception, string messageTemplate, P0 p0, P1 p1, P2 p2);
}

