using Blogger.UserStories.ReadPosts.UserTasks.ValidationTask.Sockets.ValidationSocket;
using Blogger.UserStories.ReadPosts.UserTasks.ValidationTask.Sockets.ValidationSocket.Plugins.ValidationPlugin;
using Core.UserStory;
using Microsoft.Extensions.DependencyInjection;

namespace Blogger.UserStories.ReadPosts.UserTasks.ValidationTask;

public static class Extensions
{
    public static IServiceCollection AddValidationTask(this IServiceCollection services)
    {
        services.AddScoped<IUserTask<Request, Response>, ValidationTask>();
        services.AddScoped<IValidationSocket, ValidationSocket>();
        services.AddScoped<IValidationPlugin, ValidationPlugin>();

        return services;
    }
}
