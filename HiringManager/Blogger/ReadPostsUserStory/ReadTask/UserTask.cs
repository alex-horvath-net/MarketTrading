namespace Users.Blogger.ReadPostsUserStory.ReadTask;

public class UserTask(IDataAccessSocket socket) : IUserTask<Request, Response>
{
    public async Task<bool> Run(Response response, CancellationToken token)
    {
        response.Posts = await socket.Read(response.Request, token);
        return false;
    }
}
