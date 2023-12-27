using Microsoft.Extensions.DependencyInjection;

namespace Users.Blogger.UserStories.ReadPosts.UserTasks.ReadTask.Sockets.DataAccessSocket.Plugins.DataAccessPlugin;

public static class Extensions
{
    public static IServiceCollection AddDataAccessPlugin(this IServiceCollection services)
    {
        services.AddScoped<IDataAccessPlugin, DataAccessPlugin>();

        return services;
    }
}
