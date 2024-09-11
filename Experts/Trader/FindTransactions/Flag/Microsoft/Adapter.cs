
namespace Experts.Trader.FindTransactions.Flag.Microsoft;

public class Adapter(Adapter.IClient client) : Service. IFlag
{
    public bool IsPublic(Service.Request request, CancellationToken token) => client.IsEnabled();

    public interface IClient {
        bool IsEnabled();
    }
}
