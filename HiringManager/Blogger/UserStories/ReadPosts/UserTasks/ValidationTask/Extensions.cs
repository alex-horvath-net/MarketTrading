using Core.Enterprise.UserStory;
using Microsoft.Extensions.DependencyInjection;
using Users.Blogger.UserStories.ReadPosts;
using Users.Blogger.UserStories.ReadPosts.UserTasks.ValidationTask.Sockets.ValidationSocket;
using Users.Blogger.UserStories.ReadPosts.UserTasks.ValidationTask.Sockets.ValidationSocket.Plugins.ValidationPlugin;

namespace Users.Blogger.UserStories.ReadPosts.UserTasks.ValidationTask;

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
