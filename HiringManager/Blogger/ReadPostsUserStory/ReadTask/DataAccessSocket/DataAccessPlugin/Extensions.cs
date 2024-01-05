using Microsoft.Extensions.DependencyInjection;

namespace Users.Blogger.ReadPostsUserStory.ReadTask.DataAccessSocket.DataAccessPlugin;

public static class Extensions
{
    public static IServiceCollection AddDataAccessPlugin(this IServiceCollection services)
    {
        services.AddScoped<DataAccessSocket.IDataAccessPlugin, DataAccessPlugin>();

        return services;
    }
}
