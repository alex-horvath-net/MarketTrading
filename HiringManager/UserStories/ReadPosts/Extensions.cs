using Blogger.UserStories.ReadPosts.Tasks.ReadTask;
using Blogger.UserStories.ReadPosts.Tasks.ReadTask.Sockets.DataAccessSocket;
using Blogger.UserStories.ReadPosts.Tasks.ReadTask.Sockets.DataAccessSocket.Plugins.DataAccessPlugin;
using Blogger.UserStories.ReadPosts.Tasks.ValidationTask;
using Blogger.UserStories.ReadPosts.Tasks.ValidationTask.Sockets.ValidationSocket;
using Blogger.UserStories.ReadPosts.Tasks.ValidationTask.Sockets.ValidationSocket.Plugins.ValidationPlugin;
using Core.UserStory;
using Microsoft.Extensions.DependencyInjection;

namespace Blogger.UserStories.ReadPosts;

public static class Extensions
{
    public static IServiceCollection AddReadPosts(this IServiceCollection services)
    {
        services.AddScoped<ITask<Request, Response>, ReadPostsTask>();
        services.AddScoped<IDataAccessSocket, DataAccessSocket>();
        services.AddScoped<IDataAccessPlugin, DataAccessPlugin>();

        services.AddScoped<ITask<Request, Response>, ValidationTask>();
        services.AddScoped<IValidationSocket, ValidationSocket>();
        services.AddScoped<IValidationPlugin, ValidationPlugin>();

        return services;
    }
}
