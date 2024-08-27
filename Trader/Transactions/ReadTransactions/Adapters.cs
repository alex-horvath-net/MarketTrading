

using Common.Adapters.AppDataModel;

namespace Trader.Transactions.ReadTransactions;
public class Adapters {
    public class Repository(Repository.IRepository plugin) : Business.IRepository {
        public async Task<List<Common.Business.Transaction>> Read(Business.Request request, CancellationToken token) {
            var pluginData = await plugin.Read(token);
            var domainData = pluginData.Select(ToDomain).ToList();
            return domainData;
        }

        private Common.Business.Transaction ToDomain(Transaction data) => new() {
            Id = data.Id
        };

        public interface IRepository {
            public Task<List<Transaction>> Read(CancellationToken token);
        }
    }
}
