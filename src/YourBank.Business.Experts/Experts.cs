using Business.Experts.IdentityManager;
using Business.Experts.Trader;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Experts;
public record Experts(
    Trader.Trader Trader,
    IdentityManager.IdentityManager IdentityManager);

public static class ExpertsExtensions {
    public static IServiceCollection AddExperts(this IServiceCollection services, ConfigurationManager config) => services
        .AddScoped<Experts>()
        .AddIdentityManager(config)
        .AddTrader(config);

}