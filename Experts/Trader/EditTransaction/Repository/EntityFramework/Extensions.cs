using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.EditTransaction.Repository.EntityFramework;

public static class Extensions {
    public static IServiceCollection AddRepository(this IServiceCollection services, ConfigurationManager configuration) => services
        .AddScoped<Service.IRepository, Adapter>()
        .AddScoped<Adapter.IClient, Client>()
        .AddDbContext<Common.Technology.EF.App.AppDB>(builder => builder.UseSqlServer(configuration.GetConnectionString("App")));
}
