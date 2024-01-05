using Microsoft.Extensions.DependencyInjection;

namespace Users.Blogger.UserStories.ReadPostsUserStory.UserTasks.ReadTask.Sockets.DataAccessSocket.Plugins.DataAccessPlugin;

public static class Extensions
{
    public static IServiceCollection AddDataAccessPlugin(this IServiceCollection services)
    {
        services.AddScoped<DataAccessSocket.IDataAccessPlugin, DataAccessPlugin>();

        return services;
    }
}
