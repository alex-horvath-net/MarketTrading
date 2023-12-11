using BloggerUserRole.ReadPostsUserStory.ReadTask;
using BloggerUserRole.ReadPostsUserStory.ReadTask.DataAccessSocket;
using BloggerUserRole.ReadPostsUserStory.ReadTask.DataAccessSocket.DataAccessPlugin;
using BloggerUserRole.ReadPostsUserStory.ValidationTask;
using BloggerUserRole.ReadPostsUserStory.ValidationTask.ValidationSocket;
using BloggerUserRole.ReadPostsUserStory.ValidationTask.ValidationSocket.ValidationPlugin;
using Core.UserStory;
using Microsoft.Extensions.DependencyInjection;

namespace BloggerUserRole.ReadPostsUserStory;

public static class Extensions
{
    public static IServiceCollection AddReadPosts(this IServiceCollection services)
    {
        services.AddScoped(typeof(IUserStory<,>), typeof(UserStoryCore<,>));

        services.AddScoped<ITask<Request, Response>, ReadPostsTask>();
        services.AddScoped<IDataAccessSocket, DataAccessSocket>();
        services.AddScoped<IDataAccessPlugin, DataAccessPlugin>();

        services.AddScoped<ITask<Request, Response>, ValidationTask.ValidationTask>();
        services.AddScoped<IValidationSocket, ValidationSocket>();
        services.AddScoped<IValidationPlugin, ValidationPlugin>();

        return services;
    }
}
