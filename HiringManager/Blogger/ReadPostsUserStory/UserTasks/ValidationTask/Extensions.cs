using Core.Enterprise.UserStory;
using Microsoft.Extensions.DependencyInjection;
using Users.Blogger.ReadPostsUserStory;
using Users.Blogger.ReadPostsUserStory.UserTasks.ValidationTask.Sockets.ValidationSocket;
using Users.Blogger.ReadPostsUserStory.UserTasks.ValidationTask.Sockets.ValidationSocket.Plugins.ValidationPlugin;

namespace Users.Blogger.ReadPostsUserStory.UserTasks.ValidationTask;

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
