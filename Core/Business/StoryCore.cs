using Core.Business.Model;
using Core.Solutions.Validation;

namespace Core.Business;

public interface IUserStoryCore<TRequest, TResponse, TSettings>
    where TRequest : RequestCore
    where TResponse : ResponseCore<TRequest>, new()
    where TSettings : SettingsCore {
    Task<TResponse> Run(TRequest request, CancellationToken token);
}

public class UserStoryCore<TRequest, TResponse, TSettings>
    where TRequest : RequestCore
    where TResponse : ResponseCore<TRequest>, new()
    where TSettings : SettingsCore {

    public UserStoryCore(
    IPresenter<TRequest, TResponse> presenter,
    IValidator<TRequest> validator,
    ISettings<TSettings> settings,
    ILog log,
    ITime time) {
        this.presenter = presenter;
        this.validator = validator;
        this.settings = settings;
        this.log = log;
        this.time = time;

        workSteps.Add(new StartUserWorkStep<TRequest, TResponse, TSettings>(log, time));
        workSteps.Add(new FeatureActivationUserWorkStep<TRequest, TResponse, TSettings>(settings, log, time));
        workSteps.Add(new ValidationUserWorkStep<TRequest, TResponse, TSettings>(validator, log, time));
        workSteps.Add(new StopUserWorkStep<TRequest, TResponse, TSettings>(log, time));
    }

    public async Task<TResponse> Run(TRequest request, Func<TResponse, CancellationToken, Task> Run, CancellationToken token) {
        var response = new TResponse();
        try {
            var userStoryName = GetType().Namespace ?? "";
            response.MetaData.Request = request;

            foreach (var workStep in workSteps) {
                if (!await workStep.Run(response, token)) break;
            }

            await Run(response, token);

            response.MetaData.Stoped = time.Now;
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
        log.Inform("Event {Event}, Story {Story}, Time {Time}", "StartedAt", userStoryName, now);
        return now;
    }
    private string userStoryName;
    private readonly List<IUserWorkStep<TRequest, TResponse, TSettings>> workSteps = [];
    private readonly IPresenter<TRequest, TResponse> presenter;
    private readonly IValidator<TRequest> validator;
    private readonly ISettings<TSettings> settings;
    private readonly ILog log;
    private readonly ITime time;
}

