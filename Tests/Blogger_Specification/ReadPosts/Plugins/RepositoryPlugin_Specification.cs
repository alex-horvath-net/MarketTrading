using Microsoft.EntityFrameworkCore;
using Specifications.Blogger_Specification.ReadPosts.Business;

namespace Specifications.Blogger_Specification.ReadPosts.Plugins;

public class RepositoryPlugin_Specification
{
    [Fact]
    public async void Initialize()
    {
        var options = new DbContextOptions<Core.Plugins.BloggingContext>();
        var db = new Core.Plugins.BloggingContext(options);
        db.EnsureInitialized();
        db.EnsureInitialized();

        var unit = new Blogger.ReadPosts.Plugins.DataAccess(db);
        var title = "Title";
        var content = "Content";
        var response = await unit.Read(title, content, feature.Token);

        response.Should().OnlyContain(post => post.Title.Contains(title));
    }

    private readonly Featrue_MockBuilder feature = new();
}