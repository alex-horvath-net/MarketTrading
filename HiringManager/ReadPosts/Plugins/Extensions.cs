using Microsoft.Extensions.DependencyInjection;

namespace Blogger.ReadPosts.Plugins;

public static class Extensions
{
    public static IServiceCollection AddReadPosts(this IServiceCollection services)
    {
        services.AddScoped<Business.IFeature, Business.Feature>();

        services.AddScoped<Business.IValidatorPluginAdapter, PluginAdapters.ValidatorPluginAdapter>();
        services.AddScoped<Business.IRepositoryPluginAdapter, PluginAdapters.RepositoryPluginAdapter>();

        services.AddScoped<PluginAdapters.IRepositoryPlugin, RepositoryPlugin>();
        services.AddScoped<PluginAdapters.IValidatorPlugin, ValidatorPlugin>();

        return services;
    }
}
