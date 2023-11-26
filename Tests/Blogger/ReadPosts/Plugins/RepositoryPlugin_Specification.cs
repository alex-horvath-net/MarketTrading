using Blogger.ReadPosts.Plugins;
using Microsoft.EntityFrameworkCore;
using Core.Technology.DataAccess;
using Specifications.Blogger.ReadPosts.Business;

namespace Specifications.Blogger.ReadPosts.Plugins;

public class RepositoryPlugin_Specification
{
    [Fact]
    public async void Path_Without_Diversion()
    {
        var options = new DbContextOptions<BloggingContext>();
        var db = new BloggingContext(options);
        db.EnsureInitialized();

        var unit = new RepositoryPlugin(db);
        var title = "Title";
        var content = "Content";
        var response = await unit.Read(title, content, feature.Token);

        response.Should().OnlyContain(post => post.Title.Contains(title));
    }

    private readonly WorkFlow_MockBuilder feature = new();
}