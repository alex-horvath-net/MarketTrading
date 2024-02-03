using System.Runtime;

namespace Core.Business;

public interface IUserStoryCore<TRequest, TResponse, TSettings>
  where TRequest : RequestCore
  where TResponse : ResponseCore<TRequest>, new()
  where TSettings : SettingsCore {
  Task<TResponse> Run(TRequest request, CancellationToken token);
}

public class UserStoryCore<TRequest, TResponse, TSettings>(
  ISettings<TSettings> settings,
  IValidator<TRequest> validator,  
  ILog log,
  ITime time,
  String Name) : IUserStoryCore<TRequest, TResponse, TSettings>
  where TRequest : RequestCore
  where TResponse : ResponseCore<TRequest>, new()
  where TSettings : SettingsCore {

  public async Task<TResponse> Run(TRequest request, CancellationToken token) {
    var response = new TResponse();
    try {
      response.MetaData.StartedAt = this.Start();
      response.MetaData.Request = request;
      response.MetaData.Enabled = settings.Value.Enabled;
      if (!response.MetaData.Enabled) return response;

      response.MetaData.Results = await this.Validate(request, token);
      if (response.MetaData.Results.HasFailed()) return response;

      await Run(response, token);

      response.MetaData.CompletedAt = this.Complete();
    }
    catch (Exception ex) {

      log.Error(ex, "Event {Event}, Story {Story}, {Task}, Time {Time}", "Failed", Name, "", DateTime.UtcNow);
      throw;
    }

    return response;
  }

  private DateTime Complete() {
    var now = time.UtcNow;
    log.Inform("Event {Event}, Story {Story}, Time {Time}", "Completed", Name, now);
    return now;
  }

  private Task<IEnumerable<Result>> Validate(TRequest request, CancellationToken token) => validator.Validate(request, token);

  private DateTime Start() {
    var now = time.UtcNow;
    log.Inform("Event {Event}, Story {Story}, Time {Time}", "Started", Name, now);
    return now;
  }

  public virtual Task Run(TResponse response, CancellationToken token) => Task.CompletedTask;
}

