using AppPolicy.UserStory;
using AppPolicy.UserStory.DomainModel;

namespace AppPolicy.UserTasks;

public class SrartTask<TRequest, TResponse> : IUserTask<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response<TRequest>, new() {
    public Task Run(TResponse response, CancellationToken token) {
        response.StartedAt = DateTime.UtcNow;
        return Task.CompletedTask;
    }
}
