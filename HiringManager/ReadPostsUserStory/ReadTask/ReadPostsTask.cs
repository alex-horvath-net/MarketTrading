using Core.UserStory;

namespace BloggerUserRole.ReadPostsUserStory.ReadTask;

public class ReadPostsTask(IDataAccessSocket socket) : ITask<Request, Response>
{
    public async Task Run(Response response, CancellationToken cancellation)
    {
        response.Posts = await socket.Read(response.Request, cancellation);
    }
}

public interface IDataAccessSocket
{
    Task<List<Common.UserStory.Post>> Read(Request request, CancellationToken cancellation);
}
