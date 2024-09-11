using Common.Adapters.App.Data.Model;
using Common.Technology.EF.App;

namespace Experts.Trader.EditTransaction.Repository.EntityFramework;

public class Client(AppDB db) : Adapter.IClient {

    public ValueTask<TransactionDM?> Find(long id, CancellationToken token) => 
        db.FindAsync<TransactionDM>(id, token);

    public async Task<TransactionDM> Update(TransactionDM model, CancellationToken token) {
        db.Update<TransactionDM>(model);
        await db.SaveChangesAsync(token);
        return model;
    }
}

