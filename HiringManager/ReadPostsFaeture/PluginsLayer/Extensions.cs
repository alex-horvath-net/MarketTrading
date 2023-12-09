using BloggerUserRole.ReadPostsFaeture.AdaptersLayer;
using BloggerUserRole.ReadPostsFaeture.TasksLayer;
using BloggerUserRole.ReadPostsFaeture.TasksLayer.ValidationUnit;
using BloggerUserRole.ReadPostsFaeture.UserStoryLayer.UserStoryUnit;
using Microsoft.Extensions.DependencyInjection;
using Principals.UserStoryLayer.UserStoryUnit;

namespace BloggerUserRole.ReadPostsFaeture.PluginsLayer;

public static class Extensions
{
    public static IServiceCollection AddReadPosts(this IServiceCollection services)
    {
        services.AddScoped<IUserStory<Request, Response>, UserStory.UserStoryUnit.UserStory2>();

        services.AddScoped<ITask<Response>, TasksLayer.ValidationUnit.Validation>();
        services.AddScoped<ITask<Response>, AddPosts>();

        services.AddScoped<IValidationAdapter, AdaptersLayer.Validation>();
        services.AddScoped<IDataAccess, AdaptersLayer.DataAccess>();

        services.AddScoped<AdaptersLayer.IDataAccess, DataAccess>();
        services.AddScoped<IValidation, Validation>();

        return services;
    }
}

//--Test--------------------------------------------------
