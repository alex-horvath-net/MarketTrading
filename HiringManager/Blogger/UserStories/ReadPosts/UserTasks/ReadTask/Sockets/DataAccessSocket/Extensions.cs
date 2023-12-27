using Microsoft.Extensions.DependencyInjection;
using Users.Blogger.UserStories.ReadPosts.UserTasks.ReadTask.Sockets.DataAccessSocket.Plugins.DataAccessPlugin;

namespace Users.Blogger.UserStories.ReadPosts.UserTasks.ReadTask.Sockets.DataAccessSocket;

public static class Extensions
{
    public static IServiceCollection AddDataAccessSocket(this IServiceCollection services)
    {
        services.AddScoped<IDataAccessSocket, DataAccessSocket>();
        services.AddDataAccessPlugin();

        return services;
    }
}
