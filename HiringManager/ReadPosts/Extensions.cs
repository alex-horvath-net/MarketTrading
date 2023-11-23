using Microsoft.Extensions.DependencyInjection;

namespace Blogger.ReadPosts;

public static class Extensions
{
    public static IServiceCollection AddReadPosts(this IServiceCollection services)
    {
        services.AddScoped<Business.IFeature, Business.Feature>();

        services.AddScoped<Business.IValidatorPluginAdapter, PluginAdapters.ValidatorPluginAdapter>();
        services.AddScoped<PluginAdapters.IValidatorPlugin, Plugins.ValidatorPlugin>();

        services.AddScoped<Business.IRepositoryPluginAdapter, PluginAdapters.RepositoryPluginAdapter>();
        services.AddScoped<PluginAdapters.IRepositoryPlugin, Plugins.RepositoryPlugin>();

        return services;
    }
}
