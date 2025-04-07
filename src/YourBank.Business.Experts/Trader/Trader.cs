using Business.Domain;
using Business.Experts.Trader.EditTransaction;
using Business.Experts.Trader.FindTransactions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Experts.Trader;
public class Trader(
    IFindTransactionsAdapter findTransactions,
    IEditTransaction editTransaction) {
    public Task<ViewModel> FindTransactions(string name, string userId, CancellationToken token) => findTransactions.Execute(name, userId, token);
    public Task<EditTransactionResponse> EditTransaction(EditTransactionRequest request, CancellationToken token) => editTransaction.Execute(request, token);
}


public static class ExpertExtensions {
    public static IServiceCollection AddTrader(this IServiceCollection services, ConfigurationManager config) => services
        .AddScoped<Trader>()
        .AddFindTransactions(config)
        .AddEditTransaction(config);

}