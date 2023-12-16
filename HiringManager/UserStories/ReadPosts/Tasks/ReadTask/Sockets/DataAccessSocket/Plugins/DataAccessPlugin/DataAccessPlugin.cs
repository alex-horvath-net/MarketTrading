using Common.Sockets.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Blogger.UserStories.ReadPosts.Tasks.ReadTask.Sockets.DataAccessSocket.Plugins.DataAccessPlugin;

public class DataAccessPlugin(
    Common.Plugins.BlogDbContext db) : IDataAccessPlugin
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
}
