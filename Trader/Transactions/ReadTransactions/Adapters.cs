
using Data = Infrastructure.Data.App.Model;

namespace Trader.Transactions.ReadTransactions;
public class Adapters {
    public class Repository(Repository.IRepository plugin) : Feature.IRepository {
        public async Task<List<Domain.Transaction>> Read(Feature.Request request, CancellationToken token) {
            var pluginData = await plugin.Read(token);
            var domainData = pluginData.Select(ToDomain).ToList();
            return domainData;
        }

        private Domain.Transaction ToDomain(Data.Transaction data) => new() {
            Id = data.Id
        };

        public interface IRepository {
            public Task<List<Data.Transaction>> Read(CancellationToken token);
        }
    }
}
