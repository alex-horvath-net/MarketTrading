using Common.Adapters.AppDataModel;
using Common.Business.Model;

namespace Experts.Trader.ReadTransactions.Read;


public class Adapter(IRepository repository) {
    public async Task<List<Transaction>> ReadTransaction(Request request, CancellationToken token) {
        var dataModel = request.Name == null ? await repository.ReadTransaction(token) : await repository.ReadTransaction(request.Name, token);
        var businessModel = dataModel.Select(ToBusinessData).ToList();
        return businessModel;
    }

    private Transaction ToBusinessData(TransactionDM dataModel) =>
        new() {
            Id = dataModel.Id,
            Name = dataModel.Name
        };


}
