using Data = Common.Sockets.DataModel;
using Common.UserStory.DomainModel;

namespace Blogger.UserStories.ReadPosts.Tasks.ReadTask.Sockets.DataAccessSocket;

public class DataAccessSocket(IDataAccessPlugin plugin) : IDataAccessSocket
{
    public async Task<List<Post>> Read(Request request, CancellationToken token)
    {
        var socketModel = await plugin.Read(request.Title, request.Content, token);
        var userStoryModel = socketModel.Select(x => new Post()
        {
            Title = x.Title,
            Content = x.Content
        }).ToList();
        return userStoryModel;
    }
}

public interface IDataAccessPlugin
{
    Task<List<Data.Post>> Read(string title, string content, CancellationToken token);
}
