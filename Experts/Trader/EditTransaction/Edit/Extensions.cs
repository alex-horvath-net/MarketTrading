using Common.Adapters.App.Data;
using Common.Technology.EF;
using Common.Technology.EF.App;
using Experts.Trader.EditTransaction.Edit.Adapters;
using Experts.Trader.EditTransaction.Edit.Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.EditTransaction.Edit;

public static class Extensions {
    public static IServiceCollection AddRead(this IServiceCollection services, ConfigurationManager configuration) {
        services
            .AddScoped<IDatabaseAdapter, DatabaseAdapter>()
            .AddScoped(typeof(ICommonEFClient<>), typeof(Client<>))
            .AddDbContext<AppDB>(builder => builder.UseSqlServer(configuration.GetConnectionString("App")));

        return services;
    }
}
