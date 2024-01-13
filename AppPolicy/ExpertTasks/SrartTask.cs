using Core.ExpertStory;
using Core.ExpertStory.StoryModel;

namespace Core.ExpertTasks;

public class SrartTask<TRequest, TResponse> : IExpertTask<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response<TRequest>, new() {
    public Task Run(TResponse response, CancellationToken token) {
        response.StartedAt = DateTime.UtcNow;
        return Task.CompletedTask;
    }
}
