using Microsoft.Extensions.DependencyInjection;

namespace Blogger.ReadPosts.Plugins;

public static class Extensions
{
    public static IServiceCollection AddReadPosts(this IServiceCollection services)
    {
        services.AddScoped<Business.IFeature, Business.Feature>();

        services.AddScoped<Business.IValidationAdapter, PluginAdapters.ValidationAdapter>();
        services.AddScoped<Business.IDataAccessAdapter, PluginAdapters.DataAccessAdapter>();

        services.AddScoped<PluginAdapters.IDataAccess, Plugins.DataAccess>();
        services.AddScoped<PluginAdapters.IValidation, Plugins.Validation>();

        return services;
    }
}
