using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Users.Blogger.ReadPostsUserStory.ReadUserTask.DataAccessSocket;

namespace Users.Blogger.ReadPostsUserStory.ReadUserTask.DataAccessSocket.DataAccessPlugin;

public class Plugin(BlogDbContext db) : IDataAccessPlugin
{
    public async Task<List<DataModel.Post>> Read(string title, string content, CancellationToken token)
    {
        var pluginModel = await db
            .Posts
            .Where(post => post.Title.Contains(title) || post.Content.Contains(content))
            .ToListAsync(token);

        var dataModel = pluginModel;
        return dataModel;
    }
}

public static class PluginExtensions
{
    public static IServiceCollection AddDataAccessPlugin(this IServiceCollection services) => services
        .AddScoped<IDataAccessPlugin, Plugin>();
}
