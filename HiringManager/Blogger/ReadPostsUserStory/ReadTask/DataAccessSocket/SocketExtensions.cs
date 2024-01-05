using Microsoft.Extensions.DependencyInjection;
using Users.Blogger.ReadPostsUserStory.ReadTask.DataAccessSocket.DataAccessPlugin;

namespace Users.Blogger.ReadPostsUserStory.ReadTask.DataAccessSocket;

public static class SocketExtensions
{
    public static IServiceCollection AddDataAccessSocket(this IServiceCollection services)
    {
        services.AddScoped<IDataAccessSocket, Socket>();
        services.AddDataAccessPlugin();

        return services;
    }
}
