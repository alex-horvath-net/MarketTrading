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
            response.StartedAt = Start();
            response.Request = request;
            response.Enabled = true;
            if (!response.Enabled)
                return response;

            response.Issues = await Validate(request, token);
            if (response.Issues.HasIssue())
                return response;

            await RunCore(response, token);

            response.CompletedAt  = Complete();
        } catch (Exception ex) {
            logger.LogError(ex, "Event {Event}, Story {Story}, Time {Time}", "Failed", Name, DateTime.UtcNow);
            throw;
        }

        return response;
    }

    private DateTime Complete() {
        var now = DateTime.UtcNow;
        logger.LogInformation("Event {Event}, Story {Story}, Time {Time}", "Completed", Name, now);
        return now;
    }

    private Task<IEnumerable<ValidationResult>> Validate(TRequest request, CancellationToken token) => validator.Validate(request, token);

    private DateTime Start() {
        var now = DateTime.UtcNow;
        logger.LogInformation("Event {Event}, Story {Story}, Time {Time}", "Started", Name, now);
        return now;
    }

    public virtual Task RunCore(TResponse response, CancellationToken token) => Task.CompletedTask;
}

