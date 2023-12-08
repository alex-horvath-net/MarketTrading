using Microsoft.EntityFrameworkCore;

namespace Blogger.ReadPosts.Plugins;

public class DataAccess(Assistant.Plugins.BloggingContext db) : Adapters.IDataAccess
{
    public async Task<List<App.Adapters.Post>> Read(string title, string content, CancellationToken cancelation)
    {
        var technology = await db.Posts
            .Where(post => post.Title.Contains(title) || post.Content.Contains(content))
            .ToListAsync(cancelation);
        var adapter = technology;
        return adapter;
    }
}

