using Core.Story;
using Core.Story.StoryModel;

namespace Core.ExpertTasks;

public class EndTask<TRequest, TResponse> : IProblem<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response<TRequest>, new() {
    public Task Run(TResponse response, CancellationToken token) {
        response.EndAt = DateTime.UtcNow;
        return Task.CompletedTask;
    }
}
