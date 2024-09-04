using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.ReadTransactions.Read;

public static class Extensions
{
    public static IServiceCollection AddRead(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddScoped<Adapter>()
            .AddScoped<IRepository, Repository>()
                .AddDbContext<Common.Technology.AppData.AppDB>(builder => builder.UseSqlServer(configuration.GetConnectionString("App")));

        return services;
    }
}
