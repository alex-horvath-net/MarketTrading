using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions;

public class Flag(Flag.IClient client) : Service.IFlag
{
    public bool IsPublic(Request request, CancellationToken token)
    {

        var isPublic = client.IsEnabled();
        token.ThrowIfCancellationRequested();
        return isPublic;
    }
    public interface IClient
    {
        bool IsEnabled();
    }

    public class Client : IClient
    {
        public bool IsEnabled() => false;
    }
}

public static class FlagExtensions
{

    public static IServiceCollection AddFlag(this IServiceCollection services) => services
        .AddScoped<Service.IFlag, Flag>()
        .AddFlagClient();

    public static IServiceCollection AddFlagClient(this IServiceCollection services) => services
       .AddScoped<Flag.IClient, Flag.Client>();
}