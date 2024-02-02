namespace Core.Business;

public interface IUserStoryCore<TRequest, TResponse>
  where TRequest : RequestCore
  where TResponse : ResponseCore<TRequest>, new() {
  Task<TResponse> Run(TRequest request, CancellationToken token);
}

public class StoryCore<TRequest, TResponse>(  IValidator<TRequest> validator,  ILogger logger,  String Name) : IUserStoryCore<TRequest, TResponse>
  where TRequest : RequestCore
  where TResponse : ResponseCore<TRequest>, new() {


  public async Task<TResponse> Run(TRequest request, CancellationToken token) {
    var response = new TResponse();
    try {
      return await TryRun(request, response, token);
    }
    catch (Exception ex) {
      
      logger.Error(ex, "Event {Event}, Story {Story}, {Task}, Time {Time}", "Failed",Name, "", DateTime.UtcNow);
      throw;
    }

    return response;
  }

  private async Task<TResponse> TryRun(TRequest request, TResponse response, CancellationToken token) {
    response.MetaData.StartedAt = Start();
    response.MetaData.Request = request;
    response.MetaData.Enabled = true;
    if (!response.MetaData.Enabled) return response;

    response.MetaData.Results = await Validate(request, token);
    if (response.MetaData.Results.HasFailed()) return response;

    await Run(response, token);

    response.MetaData.CompletedAt = Complete();
    return response;
  }

  private DateTime Complete() {
    var now = DateTime.UtcNow;
    logger.Inform("Event {Event}, Story {Story}, Time {Time}", "Completed", Name, now);
    return now;
  }

  private Task<IEnumerable<Result>> Validate(TRequest request, CancellationToken token) => validator.Validate(request, token);

  private DateTime Start() {
    var now = DateTime.UtcNow;
    logger.Inform("Event {Event}, Story {Story}, Time {Time}", "Started", Name, now);
    return now;
  }

  public virtual Task Run(TResponse response, CancellationToken token) => Task.CompletedTask;
}

