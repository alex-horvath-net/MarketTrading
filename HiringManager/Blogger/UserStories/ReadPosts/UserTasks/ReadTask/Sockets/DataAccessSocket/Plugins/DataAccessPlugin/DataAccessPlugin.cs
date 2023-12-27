using Core.Application.Sockets.DataModel;
using Microsoft.EntityFrameworkCore;
using Users.Blogger.UserStories.ReadPosts.UserTasks.ReadTask.Sockets.DataAccessSocket;

namespace Users.Blogger.UserStories.ReadPosts.UserTasks.ReadTask.Sockets.DataAccessSocket.Plugins.DataAccessPlugin;

public class DataAccessPlugin(
    Core.Application.Plugins.BlogDbContext db) : IDataAccessPlugin
{
    public async Task<List<Post>> Read(
        string title,
        string content,
        CancellationToken token)
    {
        var pluginModel = await db
            .Posts
            .Where(post => post.Title.Contains(title) || post.Content.Contains(content))
            .ToListAsync(token);

        var socketModel = pluginModel;
        return socketModel;
    }

    internal Task Read(string title, string content, object token)
    {
        throw new NotImplementedException();
    }
}
