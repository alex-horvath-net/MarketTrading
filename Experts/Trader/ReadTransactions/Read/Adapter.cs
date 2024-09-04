using Common.Business.Model;

namespace Experts.Trader.ReadTransactions.Read;

public class Adapter(IRepository repository) {
    public async Task<List<Transaction>> ReadTransaction(Request request, CancellationToken token) {
        var technologyModelList = request.Name == null ? await repository.ReadTransaction(token) : await repository.ReadTransaction(request.Name, token);
        var businessModelList = technologyModelList.Select(technologyModel => new Transaction() {
            Id = technologyModel.Id,
            Name = technologyModel.Name
        }).ToList();
        return businessModelList;
    }

}
