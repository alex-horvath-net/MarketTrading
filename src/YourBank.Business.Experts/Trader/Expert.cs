using Business.Domain;
using Business.Experts.Trader.EditTransaction;
using Business.Experts.Trader.FindTransactions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Experts.Trader;
public class Expert(IFindTransactions findTransactions, IEditTransaction editTransaction) {
    public Task<FindTransactionsResponse> FindTransactions(FindTransactionsRequest request, CancellationToken token) => findTransactions.Execute(request, token);
    public Task<EditTransactionResponse> EditTransaction(EditTransactionRequest request, CancellationToken token) => editTransaction.Execute(request, token);
}


public static class ExpertExtensions {
    public static IServiceCollection AddTrader(this IServiceCollection services, ConfigurationManager config) => services
        .AddScoped<Expert>()
        .AddFindTransactions(config)
        .AddEditTransaction(config);

}