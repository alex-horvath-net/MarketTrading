using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions.Repository;

public static class Extensions {

    public static IServiceCollection AddRepository(this IServiceCollection services, ConfigurationManager configuration) => services
        .AddScoped<IRepository, EFAdapter>()
        .AddScoped<EFAdapter.IEFClient, EFClient>()
        .AddDbContext<Common.Technology.EF.App.AppDB>(builder => builder.UseSqlServer(configuration.GetConnectionString("App")));
}
