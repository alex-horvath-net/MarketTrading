using Business.Experts.Trader.EditTransaction;
using Business.Experts.Trader.FindTransactions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Experts.Trader;
public class Trader(
    FindTransactions.IFeatureAdapter findTransactions,
    IEditTransaction editTransaction) {
    public Task<FindTransactions.ViewModel> FindTransactions(FindTransactions.InputModel input, CancellationToken token) => findTransactions.Execute(input, token);
    public Task<EditTransactionResponse> EditTransaction(EditTransactionRequest request, CancellationToken token) => editTransaction.Execute(request, token);
}


public static class ExpertExtensions {
    public static IServiceCollection AddTrader(this IServiceCollection services, ConfigurationManager config) => services
        .AddScoped<Trader>()
        .AddFindTransactions(config)
        .AddEditTransaction(config);

}