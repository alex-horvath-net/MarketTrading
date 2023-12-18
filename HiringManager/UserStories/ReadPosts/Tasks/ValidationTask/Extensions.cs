using Blogger.UserStories.ReadPosts.Tasks.ValidationTask.Sockets.ValidationSocket;
using Blogger.UserStories.ReadPosts.Tasks.ValidationTask.Sockets.ValidationSocket.Plugins.ValidationPlugin;
using Core.UserStory;
using Microsoft.Extensions.DependencyInjection;

namespace Blogger.UserStories.ReadPosts.Tasks.ValidationTask;

public static class Extensions
{
    public static IServiceCollection AddValidationTask(this IServiceCollection services)
    {
        services.AddScoped<ITask<Request, Response>, ValidationTask>();
        services.AddScoped<IValidationSocket, ValidationSocket>();
        services.AddScoped<IValidationPlugin, ValidationPlugin>();

        return services;
    }
}
