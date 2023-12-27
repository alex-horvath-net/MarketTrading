using Core.Application.UserStory.DomainModel;
using Core.Enterprise.UserStory;

namespace Users.Blogger.UserStories.ReadPosts.UserTasks.ReadTask;

public class ReadPostsTask(IDataAccessSocket socket) : IUserTask<Request, Response>
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
