using Blogger.ReadPosts.Adapters.ValidationUnit;
using Microsoft.Extensions.DependencyInjection;

namespace Blogger.ReadPosts.Plugins.ValidationUnit;

public static class Extensions
{
    public static IServiceCollection AddValidationPlugin(this IServiceCollection services)
    {
        services.AddScoped<IValidationPlugin, ValidationPlugin>();

        return services;
    }
}
