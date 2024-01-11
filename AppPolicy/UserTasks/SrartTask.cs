using Core.UserStory;
using Core.UserStory.DomainModel;

namespace Core.UserTasks;

public class SrartTask<TRequest, TResponse> : IScope<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response<TRequest>, new() {
    public Task Run(TResponse response, CancellationToken token) {
        response.StartedAt = DateTime.UtcNow;
        return Task.CompletedTask;
    }
}
