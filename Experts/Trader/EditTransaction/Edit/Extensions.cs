using Common.Data.Adapters;
using Common.Data.Technology;
using Experts.Trader.EditTransaction.Edit.Adapters;
using Experts.Trader.EditTransaction.Edit.Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.EditTransaction.Edit;

public static class Extensions {
    public static IServiceCollection AddRead(this IServiceCollection services, ConfigurationManager configuration) {
        services
            .AddScoped<IRepositoryAdapter, RepositoryAdapter>()
            .AddScoped(typeof(IDataClient<>), typeof(DataClient<>))
            .AddDbContext<AppDB>(builder => builder.UseSqlServer(configuration.GetConnectionString("App")));

        return services;
    }
}
