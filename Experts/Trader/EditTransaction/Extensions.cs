using Experts.Trader.EditTransaction.Edit;
using Experts.Trader.EditTransaction.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.EditTransaction;

public static class Extensions {
    public static IServiceCollection AddEditTransaction(this IServiceCollection services, ConfigurationManager configuration) {
        services.AddScoped<Service>();
        services.AddValidation();
        services.AddRead(configuration);
        return services;
    }
}
