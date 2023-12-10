using BloggerUserRole.ReadPostsFaeture.AdaptersLayer.DataAccessUnit;
using BloggerUserRole.ReadPostsFaeture.AdaptersLayer.ValidationUnit;
using BloggerUserRole.ReadPostsFaeture.PluginsLayer.DataAccessUnit;
using BloggerUserRole.ReadPostsFaeture.PluginsLayer.ValidationUnit;
using BloggerUserRole.ReadPostsFaeture.TasksLayer.DataAccessUnit;
using BloggerUserRole.ReadPostsFaeture.TasksLayer.ValidationUnit;
using BloggerUserRole.ReadPostsFaeture.UserStoryLayer.UserStoryUnit;
using Core.UserStoryLayer.UserStoryUnit;
using Microsoft.Extensions.DependencyInjection;

namespace BloggerUserRole.ReadPostsFaeture.PluginsLayer;

public static class Extensions
{
    public static IServiceCollection AddReadPosts(this IServiceCollection services)
    {
        services.AddScoped<IUserStory<Request, Response>, UserStory>();

        services.AddScoped<ITask, ValidationTask>();
        services.AddScoped<ITask, AddPostsTask>();

        services.AddScoped<IValidationAdapter, ValidationAdapter>();
        services.AddScoped<IDataAccessAdapter, DataAccessAdapter>();

        services.AddScoped<IDataAccessPlugin, DataAccessPlugin>();
        services.AddScoped<IValidationPlugin, ValidationPlugin>();

        return services;
    }
}
