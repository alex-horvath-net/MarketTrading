using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions.WorkSteps;

public class Flag(Flag.IClient client) : BusinessNeed.IFlag
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
        .AddScoped<BusinessNeed.IFlag, Flag>()
        .AddScoped<Flag.IClient, Flag.Client>();
}