using Blogger.ReadPosts.Business;
using Microsoft.Extensions.DependencyInjection;

namespace Blogger.ReadPosts;

public static class Extensions
{
    public static IServiceCollection AddReadPosts(this IServiceCollection services)
    {
        services.AddScoped<IFeature, WorkFlow>();

        services.AddScoped<IValidatorPluginAdapter, PluginAdapters.ValidatorPluginAdapter>();
        services.AddScoped<PluginAdapters.IValidatorPlugin, Plugins.ValidatorPlugin>();

        services.AddScoped<IRepositoryPluginAdapter, PluginAdapters.RepositoryPluginAdapter>();
        services.AddScoped<PluginAdapters.IRepositoryPlugin, Plugins.RepositoryPlugin>();

        return services;
    }
}
