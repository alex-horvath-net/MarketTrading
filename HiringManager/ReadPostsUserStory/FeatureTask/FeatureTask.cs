using Core.UserStory;

namespace BloggerUserRole.ReadPostsUserStory.FeatureTask;

public class FeatureTask : ITask<Request, Response>
{
    public Task Run(Response response, CancellationToken cancellation)
    {
        response.FeatureEnabled = false;
        return Task.CompletedTask;
    }
}
