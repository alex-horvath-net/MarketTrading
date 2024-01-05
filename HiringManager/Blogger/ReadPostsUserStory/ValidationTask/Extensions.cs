using Microsoft.Extensions.DependencyInjection;
using Users.Blogger.ReadPostsUserStory.ValidationTask.ValidationSocket.ValidationPlugin;

namespace Users.Blogger.ReadPostsUserStory.ValidationTask;

public static class Extensions
{
    public static IServiceCollection AddValidationTask(this IServiceCollection services)
    {
        services.AddScoped<IUserTask<Request, Response>, ValidationTask>();
        services.AddScoped<ValidationTask.IValidationSocket, ValidationSocket.ValidationSocket>();
        services.AddScoped<ValidationSocket.ValidationSocket.IValidationPlugin, ValidationPlugin>();

        return services;
    }
}
