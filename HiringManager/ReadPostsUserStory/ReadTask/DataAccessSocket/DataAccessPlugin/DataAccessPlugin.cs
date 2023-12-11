using Microsoft.EntityFrameworkCore;

namespace BloggerUserRole.ReadPostsUserStory.ReadTask.DataAccessSocket.DataAccessPlugin;

public class DataAccessPlugin(Common.Plugins.BlogDbContext db) : IDataAccessPlugin
{
    public async Task<List<Common.Sockets.DataAccess.Post>> Read(string title, string content, CancellationToken cancelation)
    {
        var technology = await db
            .Posts
            .Where(post => post.Title.Contains(title) || post.Content.Contains(content))
            .ToListAsync(cancelation);

        var adapter = technology;
        return adapter;
    }    
}
