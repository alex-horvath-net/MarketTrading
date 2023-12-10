using BloggerUserRole.ReadPostsFaeture.UserStoryLayer.UserStoryUnit;

namespace BloggerUserRole.ReadPostsFaeture.TasksLayer.DataAccessUnit;

public class AddPostsTask(IDataAccessAdapter dataAccess) : ITask
{
    public async Task Run(Response response, CancellationToken cancellation)
    {
        response.Posts = await dataAccess.Read(response.Request, cancellation);
    }
}
