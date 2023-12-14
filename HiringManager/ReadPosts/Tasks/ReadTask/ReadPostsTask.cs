namespace Blogger.ReadPosts.Tasks.ReadTask;

public class ReadPostsTask(IDataAccessSocket socket) : Core.UserStory.ITask<Request, Response>
{
    public async Task Run(
        Response response,
        CancellationToken token)
    {
        response.Posts = await socket.Read(response.Request, token);
    }
}

public interface IDataAccessSocket
{
    Task<List<Common.UserStory.Post>> Read(
        Request request,
        CancellationToken token);
}
