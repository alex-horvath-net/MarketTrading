using Core.Business.Model;
using Core.Solutions.Validation;

namespace Core.Business;

public interface IUserStoryCore<TRequest, TResponse, TSettings>
  where TRequest : RequestCore
  where TResponse : ResponseCore<TRequest>, new()
  where TSettings : SettingsCore {
    Task<TResponse> Run(TRequest request, CancellationToken token);
}

public class UserStoryCore<TRequest, TResponse, TSettings>(
  IValidator<TRequest> validator,
  ISettings<TSettings> settings,
  ILog log,
  ITime time,
  String Name) 
  where TRequest : RequestCore
  where TResponse : ResponseCore<TRequest>, new()
  where TSettings : SettingsCore {

    public async Task<TResponse> Run(TRequest request, Func<TResponse, CancellationToken, Task> Run, CancellationToken token) {
        var response = new TResponse();
        try {
            response.MetaData.Start = this.Start();
            response.MetaData.Request = request;

            response.MetaData.Enabled = settings.Value.Enabled;
            if (!response.MetaData.Enabled) {
                response.MetaData.Stop = this.Stop();
                return response;
            }

            response.MetaData.RequestIssues = await this.Validate(request, token);
            if (response.MetaData.RequestIssues.HasFailed()) {
                response.MetaData.Stop = this.Stop();
                return response;
            }

            await Run(response, token);

            response.MetaData.Stop = this.Stop();

        }
        catch (Exception ex) {
            log.Error(ex, "Event {Event}, Story {Story}, {Task}, Time {Time}", "Failed", Name, "", DateTime.UtcNow);
            throw;
        }

        return response;
    }

    private DateTime Stop() {
        var now = time.UtcNow;
        log.Inform("Event: {Event}, Story: {Story}, Time: {Time}", "Stopped", Name, now);
        return now;
    }

    private Task<IEnumerable<Result>> Validate(TRequest request, CancellationToken token) => validator.Validate(request, token);

    private DateTime Start() {
        var now = time.UtcNow;
        log.Inform("Event {Event}, Story {Story}, Time {Time}", "Started", Name, now);
        return now;
    }
}

