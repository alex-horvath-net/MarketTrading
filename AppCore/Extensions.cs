using Common.Plugins.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common;

public static class Extensions {
    public static IServiceCollection AddCoreApplication(this IServiceCollection services, IConfiguration configuration, bool isDevelopment = false) {
        services.AddDataBase(configuration, isDevelopment);

        return services;
    }
}
