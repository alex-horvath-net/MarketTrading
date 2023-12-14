using Blogger.ReadPosts.Tasks.ReadTask;
using Blogger.ReadPosts.Tasks.ReadTask.DataAccessSocket;
using Blogger.ReadPosts.Tasks.ReadTask.DataAccessSocket.DataAccessPlugin;
using Blogger.ReadPosts.Tasks.ValidationTask;
using Blogger.ReadPosts.Tasks.ValidationTask.ValidationSocket;
using Blogger.ReadPosts.Tasks.ValidationTask.ValidationSocket.ValidationPlugin;
using Core.UserStory;
using Microsoft.Extensions.DependencyInjection;

namespace Blogger.ReadPosts;

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
