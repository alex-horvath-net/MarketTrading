using Microsoft.EntityFrameworkCore;
using Shared.Adapter.DataModel;

namespace Blogger.ReadPosts.Technology;

public record Repository(Shared.Technology.Data.BloggingContext DB) : Adapters.IRepository
{
    public Task<List<Post>> Read(string title, string content, CancellationToken token) => DB.Posts
        .Where(x => x.Title.Contains(title) || x.Content.Contains(content))
        .ToListAsync(token);
}
