using BloggerUserRole.ReadPostsFaeture.AdaptersLayer.DataAccessUnit;
using Common.AdaptersLayer.DataAccessUnit;
using Microsoft.EntityFrameworkCore;

namespace BloggerUserRole.ReadPostsFaeture.PluginsLayer.DataAccessUnit;

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
