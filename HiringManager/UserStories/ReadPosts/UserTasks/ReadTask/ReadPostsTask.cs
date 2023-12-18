using Common.UserStory.DomainModel;

namespace Blogger.UserStories.ReadPosts.UserTasks.ReadTask;

public class ReadPostsTask(IDataAccessSocket socket) : Core.UserStory.IUserTask<Request, Response>
{
    public async Task<bool> Run(Response response, CancellationToken token)
    {
        response.Posts = await socket.Read(response.Request, token);
        return false;
    }
}

public interface IDataAccessSocket
{
    Task<List<Post>> Read(Request request, CancellationToken token);
}
