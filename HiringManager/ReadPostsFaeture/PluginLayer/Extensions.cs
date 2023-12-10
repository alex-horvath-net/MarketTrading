using BloggerUserRole.ReadPostsFaeture.AdapterLayer.DataAccessUnit;
using BloggerUserRole.ReadPostsFaeture.AdapterLayer.ValidationUnit;
using BloggerUserRole.ReadPostsFaeture.PluginLayer.DataAccessUnit;
using BloggerUserRole.ReadPostsFaeture.PluginLayer.ValidationUnit;
using BloggerUserRole.ReadPostsFaeture.TaskLayer.DataAccessUnit;
using BloggerUserRole.ReadPostsFaeture.TaskLayer.ValidationUnit;
using BloggerUserRole.ReadPostsFaeture.UserStoryLayer.UserStoryUnit;
using Core.UserStoryLayer.UserStoryUnit;
using Microsoft.Extensions.DependencyInjection;

namespace BloggerUserRole.ReadPostsFaeture.PluginLayer;

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
