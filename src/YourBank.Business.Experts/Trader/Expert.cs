using Business.Domain;
using Business.Experts.Trader.EditTransaction;
using Business.Experts.Trader.FindTransactions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Experts.Trader;
public class Expert {
    public IEnumerable<Trade> FindAllTransations() {
        return [new() { Name = "Name1" }];
    }
}


public static class ExpertExtensions {
    public static IServiceCollection AddTrader(this IServiceCollection services, ConfigurationManager config) => services
        .AddScoped<Expert>()
        .AddFindTransactions(config)
        .AddEditTransaction(config);

} 