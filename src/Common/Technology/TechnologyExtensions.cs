using Infrastructure.Technology.EF.App;
using Infrastructure.Technology.EF.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Technology;
public static class TechnologyExtensions {

    public static IServiceCollection AddCommonTechnology(this IServiceCollection services, ConfigurationManager configuration) => services
        .AddEntityFramework(configuration);


    private static IServiceCollection AddEntityFramework(this IServiceCollection services, ConfigurationManager configuration) {

        var appConnectionString = configuration.GetConnectionString("App") ?? throw new InvalidOperationException("App Connection string not found.");
        var identityConnectionString = configuration.GetConnectionString("Identity") ?? throw new InvalidOperationException("Identity Connection string not found.");

        return services
            .AddDbContext<AppDB>((sp, options) => options.UseSqlServer(appConnectionString))
            .AddDbContext<IdentityDB>((sp, options) => options.UseSqlServer(appConnectionString));
    }
}
