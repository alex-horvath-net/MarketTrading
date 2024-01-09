using Core.App.Plugins.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.App;

public static class Extensions
{
    public static IServiceCollection AddCoreApplication(this IServiceCollection services, IConfiguration configuration, bool isDevelopment= false)
    {
        services.AddDataBase(configuration, isDevelopment);

        return services;
    }
}
