using Blogger.ReadPosts.Adapters.DataAccessUnit;
using Microsoft.Extensions.DependencyInjection;

namespace Blogger.ReadPosts.Plugins.DataAccessUnit;

public static class Extensions
{
    public static IServiceCollection AddDataAccessPlugin(this IServiceCollection services)
    {
        services.AddScoped<IDataAccessPlugin, DataAccessPlugin>();
        return services;
    }
}
