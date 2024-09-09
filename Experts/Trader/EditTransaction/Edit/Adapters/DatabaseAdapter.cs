using Common.Data.Adapters;
using Common.Data.Business.Model;
using Experts.Trader.EditTransaction.Edit.Business;

namespace Experts.Trader.EditTransaction.Edit.Adapters;

public class DatabaseAdapter(IDataClient<TransactionDM> client) :
    IDatabaseAdapter {

    public async Task<Transaction?> EditTransaction(Request request, CancellationToken token) {
        var dm = await client.Find(request.Id, token);
        if (dm != null) {
            SetDtaModel(dm, request);
            var dataModel = await client.Update(dm, token);
            var businessModel = CreateBusinessModel(dataModel);
            return businessModel;
        }

        return null;    
    }

    private static void SetDtaModel(TransactionDM dm, Request request) {
        dm.Name = request.Name;
    }

    private static Transaction CreateBusinessModel(TransactionDM technologyModel) => new() {
        Id = technologyModel.Id,
        Name = technologyModel.Name
    };
}
 