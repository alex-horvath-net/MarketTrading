using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Story.Solutions.Data.MainDB;

namespace Story;

public static class Extensions {
    public static IServiceCollection AddCoreApplication(this IServiceCollection services, IConfiguration configuration, bool isDevelopment = false) {
        services.AddDataBase(configuration, isDevelopment);

        return services;
    }
}
