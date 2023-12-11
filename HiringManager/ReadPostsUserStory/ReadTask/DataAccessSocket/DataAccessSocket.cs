namespace BloggerUserRole.ReadPostsUserStory.ReadTask.DataAccessSocket;

public class DataAccessSocket(IDataAccessPlugin plugin) : IDataAccessSocket
{
    public async Task<List<Common.UserStory.Post>> Read(Request request, CancellationToken token)
    {
        var socketModel = await plugin.Read(request.Title, request.Content, token);
        var userStoryModel = socketModel.Select(x => new Common.UserStory.Post()
        {
            Title = x.Title,
            Content = x.Content
        }).ToList();
        return userStoryModel;
    }
}

public interface IDataAccessPlugin
{
    Task<List<Common.Sockets.DataAccess.Post>> Read(string title, string content, CancellationToken token);
}
