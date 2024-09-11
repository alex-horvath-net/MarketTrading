using Common.Adapters.App.Data.Model;
using Common.Business.Model;

namespace Experts.Trader.EditTransaction.Repository.EntityFramework;

public class Adapter(Adapter.IClient client) : Service.IRepository {
    public async Task<Transaction> EditTransaction(Service.Request request, CancellationToken token) {
        var cm = await client.Find(request.TransactionId, token);
        if (cm != null) {
            SetDtaModel(cm, request);
            var dataModel = await client.Update(cm, token);
            var businessModel = ToBusinessModel(dataModel);
            return businessModel;
        }

        return null;

        static void SetDtaModel(TransactionDM dm, Service.Request request) {
            dm.Name = request.Name;
        }
               
        static Transaction ToBusinessModel(TransactionDM dataModel) => new() {
            Id = dataModel.Id,
            Name = dataModel.Name
        };
    }



    public interface IClient {
        ValueTask<TransactionDM?> Find(long id, CancellationToken token);
        Task<TransactionDM> Update(TransactionDM model, CancellationToken token);
    }
}
