
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions.Flag.Microsoft;

public class Adapter(Adapter.IClient client) : Service. IFlag
{
    public bool IsPublic(Service.Request request, CancellationToken token) {

        var isPublic = client.IsEnabled();
        token.ThrowIfCancellationRequested();
        return isPublic;
    }
    public interface IClient {
        bool IsEnabled();
    }
}

public static class AdapterExtensions {

    public static IServiceCollection AddFlagAdapter(this IServiceCollection services) => services
        .AddScoped<Service.IFlag, Adapter>()
        .AddFlagClient();
} 