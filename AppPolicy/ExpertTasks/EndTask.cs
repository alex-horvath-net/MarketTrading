using Core.ExpertStory;
using Core.ExpertStory.DomainModel;

namespace Core.ExpertTasks;

public class EndTask<TRequest, TResponse> : IScope<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response<TRequest>, new() {
    public Task Run(TResponse response, CancellationToken token) {
        response.EndAt = DateTime.UtcNow;
        return Task.CompletedTask;
    }
}
