using Core.Enterprise.UserStory;
using Microsoft.Extensions.DependencyInjection;
using Users.Blogger.UserStories.ReadPosts;
using Users.Blogger.UserStories.ReadPosts.UserTasks.ReadTask.Sockets.DataAccessSocket;
using Users.Blogger.UserStories.ReadPosts.UserTasks.ReadTask.Sockets.DataAccessSocket.Plugins.DataAccessPlugin;

namespace Users.Blogger.UserStories.ReadPosts.UserTasks.ReadTask;

public static class Extensions
{
    public static IServiceCollection AddReadTask(this IServiceCollection services)
    {
        services.AddScoped<IUserTask<Request, Response>, ReadPostsTask>();
        services.AddScoped<IDataAccessSocket, DataAccessSocket>();
        services.AddScoped<IDataAccessPlugin, DataAccessPlugin>();

        return services;
    }
}
