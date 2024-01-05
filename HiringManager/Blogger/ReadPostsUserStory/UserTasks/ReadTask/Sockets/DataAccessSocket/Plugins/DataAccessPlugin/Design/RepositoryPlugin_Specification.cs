using Core.Application;
using Core.Application.Plugins;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Users.Blogger.ReadPostsUserStory.UserTasks.ReadTask.Sockets.DataAccessSocket.Plugins.DataAccessPlugin.Design;

public class RepositoryPlugin_Specification
{
    [Fact]
    public async void Initialize()
    {
        var options = new DbContextOptions<BlogDbContext>();
        var db = new BlogDbContext(options);
        db.EnsureInitialized();
        db.EnsureInitialized();

        var unit = new DataAccessPlugin(db);
        var title = "Title";
        var content = "Content";
        var response = await unit.Read(title, content, CancellationToken.None);

        response.Should().OnlyContain(post => post.Title.Contains(title));
    }

    [Fact]
    public void UseDataBase()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddCommon(builder.Configuration);
        var app = builder.Build();

        app.UseDataBase();
        using var scope = app.Services.CreateScope();
        using var db = scope.ServiceProvider.GetRequiredService<BlogDbContext>();

        db.Posts.Should().NotBeEmpty();
    }
}