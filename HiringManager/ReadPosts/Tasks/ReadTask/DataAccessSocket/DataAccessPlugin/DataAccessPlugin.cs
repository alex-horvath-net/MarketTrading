using Blogger.ReadPosts.Tasks.ReadTask.DataAccessSocket;
using Microsoft.EntityFrameworkCore;

namespace Blogger.ReadPosts.Tasks.ReadTask.DataAccessSocket.DataAccessPlugin;

public class DataAccessPlugin(
    Common.Plugins.BlogDbContext db) : IDataAccessPlugin
{
    public async Task<List<Common.Sockets.DataAccess.Post>> Read(
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
