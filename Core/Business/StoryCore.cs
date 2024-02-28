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
    IPresenter<TRequest, TResponse> presenter,
    IValidator<TRequest> validator,
    ISettings<TSettings> settings,
    ILog log,
    ITime time)
    where TRequest : RequestCore
    where TResponse : ResponseCore<TRequest>, new()
    where TSettings : SettingsCore {

    public async Task<TResponse> Run(TRequest request, Func<TResponse, CancellationToken, Task> Run, CancellationToken token) {
        var response = new TResponse();
        try {
            userStoryName = GetType()?.Namespace ?? "";
            response.MetaData.Start = time.Now;
            response.MetaData.Request = request;

            response.MetaData.Enabled = settings.Value.Enabled;
            if (!response.MetaData.Enabled) {
                response.MetaData.Stop = time.Now;
                return response;
            }

            response.MetaData.RequestIssues = await validator.Validate(request, token);
            if (response.MetaData.RequestIssues.HasFailed()) {
                response.MetaData.Stop = time.Now;
                return response;
            }

            await Run(response, token);

            response.MetaData.Stop = time.Now;
            presenter.Handle(response);
        }
        catch (Exception ex) {
            log.Error(ex, "Event {Event}, Story {Story}, {Task}, Time {Time}", "Failed", userStoryName, "", DateTime.UtcNow);
            throw;
        }
        return response;
    }

    private DateTime Stop2() {
        var now = time.UtcNow;
        log.Inform("Event: {Event}, Story: {Story}, Time: {Time}", "Stopped", userStoryName, now);
        return now;
    }



    private DateTime Start2() {
        var now = time.UtcNow;
        log.Inform("Event {Event}, Story {Story}, Time {Time}", "Started", userStoryName, now);
        return now;
    }
    private string userStoryName;
}

