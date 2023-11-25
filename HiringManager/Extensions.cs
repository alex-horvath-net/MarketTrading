using Blogger.ReadPosts.Plugins;
using Microsoft.Extensions.DependencyInjection;

namespace Blogger;

public static class Extensions
{
    public static IServiceCollection AddBlogger(this IServiceCollection services)
    {
        services.AddReadPosts();

        return services;
    }
}
