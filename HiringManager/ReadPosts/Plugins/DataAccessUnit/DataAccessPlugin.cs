using Blogger.ReadPosts.Adapters.DataAccessUnit;
using Common.Adapters.DataAccessUnit;
using Microsoft.EntityFrameworkCore;

namespace Blogger.ReadPosts.Plugins.DataAccessUnit;

public class DataAccessPlugin(Assistant.Plugins.BlogDbContext db) : IDataAccessPlugin
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
