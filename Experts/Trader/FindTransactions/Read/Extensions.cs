using Common.Data.Technology;
using Experts.Trader.FindTransactions.Read.Adapters;
using Experts.Trader.FindTransactions.Read.Business;
using Experts.Trader.FindTransactions.Read.Technology;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions.Read;

public static class Extensions {
    public static IServiceCollection AddRead(this IServiceCollection services, ConfigurationManager configuration) => services
        .AddScoped<IRepositoryAdapter, RepositoryAdapter>()
        .AddScoped<IRepositoryClient, RepositoryClient>()
        .AddDbContext<AppDB>(builder => builder.UseSqlServer(configuration.GetConnectionString("App")));
}
