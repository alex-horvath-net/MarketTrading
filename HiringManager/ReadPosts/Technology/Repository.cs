using Microsoft.EntityFrameworkCore;
using Shared.Adapter.DataModel;
using Shared.Technology.DataAccess;

namespace Blogger.ReadPosts.Technology;

public record Repository(BloggingContext DB) : Adapters.IRepository
{
    public Task<List<Post>> Read(string title, string content, CancellationToken token) => DB.Posts
        .Where(x => x.Title.Contains(title) || x.Content.Contains(content))
        .ToListAsync(token);
}
