using Blogger.ReadPosts.PluginAdapters;
using Core.PluginAdapters;
using Microsoft.EntityFrameworkCore;

namespace Blogger.ReadPosts.Plugins;

public class DataAccess(
    Core.Plugins.BloggingContext db) : IDataAccess
{
    public async Task<List<Post>> Read(string title, string content, CancellationToken cancelation)
    {
        var technology = await db.Posts
            .Where(post => post.Title.Contains(title) || post.Content.Contains(content))
            .ToListAsync(cancelation);
        var adapter = technology;
        return adapter;
    }
}

