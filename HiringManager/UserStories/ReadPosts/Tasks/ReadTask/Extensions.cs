using Blogger.UserStories.ReadPosts.Tasks.ReadTask.Sockets.DataAccessSocket;
using Blogger.UserStories.ReadPosts.Tasks.ReadTask.Sockets.DataAccessSocket.Plugins.DataAccessPlugin;
using Core.UserStory;
using Microsoft.Extensions.DependencyInjection;

namespace Blogger.UserStories.ReadPosts.Tasks.ReadTask;

public static class Extensions
{
    public static IServiceCollection AddReadTask(this IServiceCollection services)
    {
        services.AddScoped<ITask<Request, Response>, ReadPostsTask>();
        services.AddScoped<IDataAccessSocket, DataAccessSocket>();
        services.AddScoped<IDataAccessPlugin, DataAccessPlugin>();

        return services;
    }
}
