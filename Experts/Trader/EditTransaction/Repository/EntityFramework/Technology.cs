using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.EditTransaction.Repository.EntityFramework;

public static class TechnologyExtensions {
    public static IServiceCollection AddRepositoryTechnology(this IServiceCollection services, ConfigurationManager configuration) => services
        .AddDbContext<Common.Technology.EF.App.AppDB>(builder => builder.UseSqlServer(configuration.GetConnectionString("App")));
}
