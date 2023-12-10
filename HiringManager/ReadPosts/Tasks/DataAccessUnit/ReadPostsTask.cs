using Blogger.ReadPosts.UserStory.UserStoryUnit;

namespace Blogger.ReadPosts.Tasks.DataAccessUnit;

public class ReadPostsTask(IDataAccessAdapter dataAccess) : ITask
{
    public async Task Run(Response response, CancellationToken cancellation)
    {
        response.Posts = await dataAccess.Read(response.Request, cancellation);
    }
}
