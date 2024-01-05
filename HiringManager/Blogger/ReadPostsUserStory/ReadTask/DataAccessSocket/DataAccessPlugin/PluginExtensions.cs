using Microsoft.Extensions.DependencyInjection;

namespace Users.Blogger.ReadPostsUserStory.ReadTask.DataAccessSocket.DataAccessPlugin;

public static class PluginExtensions
{
    public static IServiceCollection AddDataAccessPlugin(this IServiceCollection services)
    {
        services.AddScoped<IDataAccessPlugin, Plugin>();

        return services;
    }
}
 