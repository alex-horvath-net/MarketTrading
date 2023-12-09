using BloggerUserRole.ReadPostsFaeture.AdaptersLayer;
using Microsoft.EntityFrameworkCore;
using Polices.AdaptersLayer;

namespace BloggerUserRole.ReadPostsFaeture.PluginsLayer;

public class DataAccess(Assistant.Plugins.BloggingContext db) : IDataAccess
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