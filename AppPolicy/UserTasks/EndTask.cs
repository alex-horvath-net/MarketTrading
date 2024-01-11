using Core.UserStory;
using Core.UserStory.DomainModel;

namespace Core.UserTasks;

public class EndTask<TRequest, TResponse> : IUserTask<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response<TRequest>, new() {
    public Task Run(TResponse response, CancellationToken token) {
        response.EndAt = DateTime.UtcNow;
        return Task.CompletedTask;
    }
}
