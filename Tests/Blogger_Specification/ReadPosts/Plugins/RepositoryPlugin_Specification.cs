using Blogger.ReadPosts.Plugins;
using Microsoft.EntityFrameworkCore;
using Core.Technology.DataAccess;
using Specifications.Blogger_Specification.ReadPosts.Business;

namespace Specifications.Blogger_Specification.ReadPosts.Plugins;

public class RepositoryPlugin_Specification
{
    [Fact]
    public async void Path_Without_Diversion()
    {
        var options = new DbContextOptions<BloggingContext>();
        var db = new BloggingContext(options);
        db.EnsureInitialized();

        var unit = new DataAccess(db);
        var title = "Title";
        var content = "Content";
        var response = await unit.Read(title, content, feature.Token);

        response.Should().OnlyContain(post => post.Title.Contains(title));
    }

    private readonly Featrue_MockBuilder feature = new();
}