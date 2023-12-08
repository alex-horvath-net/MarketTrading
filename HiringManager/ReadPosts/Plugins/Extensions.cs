using Microsoft.Extensions.DependencyInjection;

namespace Blogger.ReadPosts.Plugins;

public static class Extensions
{
    public static IServiceCollection AddReadPosts(this IServiceCollection services)
    {
        services.AddScoped<Sys.UserStory.IUserStory<UserStory.Request, UserStory.Response>, UserStory.UserStory>();

        services.AddScoped<Sys.UserStory.ITask<UserStory.Response>, Tasks.Validation>();
        services.AddScoped<Sys.UserStory.ITask<UserStory.Response>, Tasks.AddPosts>();

        services.AddScoped<Tasks.IValidation, Adapters.Validation>();
        services.AddScoped<Tasks.IDataAccess, Adapters.DataAccess>();

        services.AddScoped<Adapters.IDataAccess, Plugins.DataAccess>();
        services.AddScoped<Adapters.IValidation, Plugins.Validation>();

        return services;
    }
}
