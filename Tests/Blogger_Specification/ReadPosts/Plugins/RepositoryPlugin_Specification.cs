using Blogger;
using Core.Plugins;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
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

    [Fact]
    public void UseDataBase()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddCore(builder.Configuration);
        var app = builder.Build();                                                                     
        
        app.UseDataBase();
        using var scope = app.Services.CreateScope();
        using var db = scope.ServiceProvider.GetRequiredService<BloggingContext>();

        db.Posts.Should().NotBeEmpty();
    }

    private readonly Featrue_MockBuilder feature = new();
}