using BloggerUserRole.ReadPostsFaeture.AdaptersLayer;
using Common.AdaptersLayer.DataAccessUnit;
using Microsoft.EntityFrameworkCore;

namespace BloggerUserRole.ReadPostsFaeture.PluginsLayer;

public class DataAccess(Assistant.Plugins.BlogDbContext db) : IDataAccess
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

//--Test--------------------------------------------------