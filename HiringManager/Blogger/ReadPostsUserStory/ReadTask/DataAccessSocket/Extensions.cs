using Microsoft.Extensions.DependencyInjection;
using Users.Blogger.ReadPostsUserStory.ReadTask.DataAccessSocket.DataAccessPlugin;

namespace Users.Blogger.ReadPostsUserStory.ReadTask.DataAccessSocket;

public static class Extensions
{
    public static IServiceCollection AddDataAccessSocket(this IServiceCollection services)
    {
        services.AddScoped<ReadPostsTask.IDataAccessSocket, DataAccessSocket>();
        services.AddDataAccessPlugin();

        return services;
    }
}
