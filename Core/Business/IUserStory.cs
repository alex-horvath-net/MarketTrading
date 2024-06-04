using Core.Business.Model;

namespace Core.Business;

public interface IUserStory<TRequest, TResponse>
    where TRequest : RequestCore where TResponse : ResponseCore<TRequest>, new() {
    Task<TResponse> Run(TRequest request, CancellationToken token);
}

public class UserStory<TRequest, TResponse>(
    IEnumerable<IUserWorkStep<TRequest, TResponse>> workSteps,
    IPresenter<TRequest, TResponse> presenter, ILog log, ITime time) : IUserStory<TRequest, TResponse>
    where TRequest : RequestCore where TResponse : ResponseCore<TRequest>, new() {

    public async Task<TResponse> Run(TRequest request, CancellationToken token) {
        var response = new TResponse();
        try {
            log.Inform("UserStory: {UserStory}, Event: {Event}, Time: {Time}", Name, "Start", time.UtcNow);

            await RunUserStory(workSteps, request, response, token);
            presenter.MapUS2UI(response);

            log.Inform("UserStory: {UserStory}, Event: {Event}, Time: {Time}", Name, "End", time.UtcNow);
        }
        catch (Exception ex) {
            log.Error(ex, "UserStory: {UserStory}, Event: {EventName}, Time: {Time}", Name, "Failed", time.UtcNow);
            throw;
        }
        return response;
    }

    private async Task RunUserStory(IEnumerable<IUserWorkStep<TRequest, TResponse>> workSteps, TRequest request, TResponse response, CancellationToken token) {
        response.MetaData.Request = request;
        foreach (var workStep in workSteps.Reverse().Skip(1).Reverse()) {
            log.Debug("UserStory: {UserStory}, WorkStep: {WorkStep}, Event: {Event}, Time: {Time}", Name, workStep.Name, "Start", time.UtcNow);
            if (!await workStep.Run(response, token)) {
                log.Warning("UserStory: {UserStory}, WorkStep: {WorkStep}, Event: {Event}, Time: {Time}", Name, workStep.Name, "Break", time.UtcNow);
                break;
            }
            log.Debug("UserStory: {UserStory}, WorkStep: {WorkStep}, Event: {Event}, Time: {Time}", Name, workStep.Name, "End", time.UtcNow);
        }
        await workSteps.Last().Run(response, token);
    }

    private string name;
    private string Name => name ??= GetType().Name;
}

