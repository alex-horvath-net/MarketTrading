using Microsoft.Extensions.DependencyInjection;
using Users.Blogger.UserStories.ReadPostsUserStory.UserTasks.ReadTask;
using Users.Blogger.UserStories.ReadPostsUserStory.UserTasks.ReadTask.Sockets.DataAccessSocket.Plugins.DataAccessPlugin;

namespace Users.Blogger.UserStories.ReadPostsUserStory.UserTasks.ReadTask.Sockets.DataAccessSocket;

public static class Extensions
{
    public static IServiceCollection AddDataAccessSocket(this IServiceCollection services)
    {
        services.AddScoped<ReadPostsTask.IDataAccessSocket, DataAccessSocket>();
        services.AddDataAccessPlugin();

        return services;
    }
}
