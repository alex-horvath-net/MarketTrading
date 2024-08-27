

using Common.Business;

namespace Trader.Transactions.ReadTransactions;
public class Adapters {
    public class Repository(Repository.IRepository plugin) : Business.IRepository {
        public async Task<List<Transaction>> Read(Business.Request request, CancellationToken token) {
            var pluginData = await plugin.Read(token);
            var domainData = pluginData.Select(ToDomain).ToList();
            return domainData;
        }

        private Transaction ToDomain(Transaction data) => new() {
            Id = data.Id
        };

        public interface IRepository {
            public Task<List<Transaction>> Read(CancellationToken token);
        }
    }
}
